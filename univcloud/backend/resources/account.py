import dataclasses

from flask_restful import reqparse, Resource
from munch import Munch

from connection.connection import Connection, NoScopeableDomainOrProjectException
from resources.utils import StandardErrorDto

login_parser = reqparse.RequestParser()
login_parser.add_argument('username', type=str, required=True, help='username')
login_parser.add_argument('password', type=str, required=True, help='password')
login_parser.add_argument('domain', type=str, required=True, help='domain name')


class AccountResource(Resource):
    PATH = "/account"

    def get(self):
        args = Munch(login_parser.parse_args())
        try:
            connection = Connection.connect(username=args.username,
                                            password=args.password,
                                            domain_name=args.domain)
        except NoScopeableDomainOrProjectException:
            return StandardErrorDto(code="NoScopeableDomainOrProject"), 403

        return dict(token=connection.auth_token,
                    scope=dataclasses.asdict(connection.current_scope),
                    domain_id=connection.domain_id)
