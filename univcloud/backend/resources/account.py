from dataclasses import dataclass, asdict
from flask_restful import reqparse, Resource, fields, marshal_with
from munch import Munch

from connection.connection import get_scopeable_targets, scoped_connect, ScopedAuth
from utils.auth import generate_token, decode_token

scopeable_targets_request_parser = reqparse.RequestParser()
scopeable_targets_request_parser.add_argument('username', type=str, required=True, help='username')
scopeable_targets_request_parser.add_argument('password', type=str, required=True, help='password')
scopeable_targets_request_parser.add_argument('domainName', type=str, required=True, help='domain name')

login_parser = reqparse.RequestParser()
login_parser.add_argument("username", type=str, required=True, help="username")
login_parser.add_argument("password", type=str, required=True, help="password")
login_parser.add_argument("domainName", type=str, required=True, help="domain name")
login_parser.add_argument("projectName", type=str, required=False, help="project name, if scoped to project")


class Account(Resource):
    PATH = "/account"

    # Get scopeable targets
    def get(self):
        args = Munch(scopeable_targets_request_parser.parse_args())
        targets = get_scopeable_targets(args.username, args.password, args.domainName)
        return [asdict(target) for target in targets]

    # Login. Generate token.
    def post(self):
        args = Munch(login_parser.parse_args())
        auth = ScopedAuth(args.username, args.password, args.domainName, args.projectName)
        conn = scoped_connect(auth)
        return {
            "token": generate_token(auth),
            "scope": asdict(conn.current_scope)
        }
