from typing import TypedDict

from flask_restful import reqparse, Resource
from munch import Munch

from connection.connection import Connection, NoScopeableDomainOrProjectException

login_parser = reqparse.RequestParser()
login_parser.add_argument('username', type=str, required=True, help='username')
login_parser.add_argument('password', type=str, required=True, help='password')
login_parser.add_argument('domain', type=str, required=True, help='domain name')
login_parser.add_argument('project', type=str, required=False, help='project name. None for first login.')


class AccountResource(Resource):
    PATH = "/account"

    def get(self):
        class LoginResponse(TypedDict):
            token: str
            project_id: str

        args = Munch(login_parser.parse_args())
        try:
            connection = Connection.connect(username=args.username, password=args.password, domain_name=args.domain,
                                            project_name=args.project)
        except NoScopeableDomainOrProjectException:
            return {"error": "NoScopeableDomainOrProject"}, 403

        return LoginResponse(token=connection.auth_url, project_id=connection.current_project), 200
