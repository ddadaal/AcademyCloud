from flask import Flask
from flask_restx import Api

import config
from db import init_db
from resources import init_resources

app = Flask(__name__)
app.config['SQLALCHEMY_DATABASE_URI'] = config.database_url
app.config['RESTX_VALIDATE'] = True
init_db(app)

api = Api(app)
init_resources(api)

if __name__ == "__main__":
    app.run(debug=True)
