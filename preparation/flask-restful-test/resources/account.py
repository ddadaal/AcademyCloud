import hashlib
from dataclasses import dataclass
from typing import TypedDict

from dataclasses_json import dataclass_json
from flask_restful import Resource, reqparse, fields

from data.account import get_user_by_username, add_user
from data.entities import User, db, UserRole
from util.auth.token_utils import create_token
from util.dto.errors import StandardErrorDto

login_parser = reqparse.RequestParser()
login_parser.add_argument('username', type=str, required=True, help='username')
login_parser.add_argument('password', type=str, required=True, help='password')

register_parser = reqparse.RequestParser()
register_parser.add_argument('username', type=str, required=True, help='username')
register_parser.add_argument('password', type=str, required=True, help='password')


class LoginResponseDto(TypedDict):
    token: str
    role: UserRole
    username: str


def encode(password):
    return hashlib.md5(password.encode()).hexdigest()


class AccountResource(Resource):

    # Login
    def get(self):
        args = login_parser.parse_args()
        username = args['username']
        user = get_user_by_username(username)
        if user and user.password == encode(args['password']):
            return LoginResponseDto(token=create_token(username), role=user.role, username=username), 200
        else:
            return StandardErrorDto(code='CredentialNotValid',
                                    description='Credentials provided are invalid'), 401

    # Register
    def post(self):
        args = register_parser.parse_args()

        username = args['username']
        hashed_password = encode(args['password'])
        user = add_user(username, hashed_password)

        if user is None:
            return StandardErrorDto(code='UsernameConflict', description="Username has been token"), 409

        return LoginResponseDto(token=create_token(username), role=user.role, username=username), 200
