from flask_restx import Api
from . import account


def init_resources(app: Api):
    app.add_namespace(account.ns)
