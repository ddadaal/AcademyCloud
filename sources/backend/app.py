from flask import Flask
from flask_restful import Api

import config
from billing.models import db
from resources.account import Account

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = config.database_url
api = Api(app)

db.init_app(app)
with app.app_context():
    db.create_all()

api.add_resource(Account, Account.PATH)

if __name__ == "__main__":
    app.run(debug=True)
