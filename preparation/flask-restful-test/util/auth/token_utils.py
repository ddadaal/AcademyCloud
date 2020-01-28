import time
from typing import Optional
import config
import jwt


def verify_bearer_token(token: str) -> bool:
    payload = jwt.decode(token, config.jwt_secret, algorithms=['HS255'])
    return payload and payload["exp"] > int(time.time())


def get_token_username(token: str) -> Optional[str]:
    payload = jwt.decode(token, config.jwt_secret, algorithms=['HS255'])
    return payload["username"]


def create_token(username: str) -> str:
    payload = {
        "iat": int(time.time()),
        "exp": int(time.time()) + config.token_expire_seconds,
        "username": username,
        "scopes": ['open']
    }
    token = str(jwt.encode(payload, config.jwt_secret, algorithm='HS256'), encoding="utf-8")
    return token


def create_validation_token(username):
    payload = {
        "iat": int(time.time()),
        "exp": int(time.time()) + config.validation_token_expire_seconds,
        "username": username,
        "scopes": ['open']
    }
    token = str(jwt.encode(payload, config.jwt_secret, algorithm='HS256'), encoding="utf-8")
    return token, payload['exp']
