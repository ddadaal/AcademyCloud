from functools import wraps

from grpc import ServicerContext, StatusCode

from config import token_header

from typing import Optional, TypedDict
import config
import jwt

from utils.token_claims import TokenClaims


def get_token_claims(token: str) -> Optional[TokenClaims]:
    return jwt.decode(token, config.jwt_secret, algorithms=['HS256'], audience=config.jwt_audience,
                      options={'verify_exp': False})


def auth_required(func):
    '''
    标记在一个gRPC的方法上（比如User::get），表示这个方法需要验证是否登录（即是headers里是否有有效的Authorization: Bearer {token}）。
    如果没有登录，返回401
    否则，会给被调用函数传入user参数，即为用户对象
    :return: decorator而已
    '''

    @wraps(func)
    def wrapper(self, request, context: ServicerContext):
        metadata = context.invocation_metadata()
        token = next((x for x in metadata if x[0] == config.token_header_key), None)
        if token is None:
            context.abort(StatusCode.UNAUTHENTICATED, "Permission denied")

        token = token.value.split(token_header)[1]
        claims = get_token_claims(token)
        if claims is None:
            context.abort(StatusCode.UNAUTHENTICATED, "Permission denied")
        else:
            return func(self, request, context, claims=claims)

    return wrapper
