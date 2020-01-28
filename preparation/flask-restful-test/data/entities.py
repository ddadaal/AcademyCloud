import enum

from flask_sqlalchemy import SQLAlchemy

db = SQLAlchemy()


class UserRole(str, enum.Enum):
    admin = "admin"
    user = "user"


class User(db.Model):
    __tablename__ = 'users'

    id = db.Column(db.Integer, primary_key=True)
    username = db.Column(db.String, unique=True, nullable=False)
    password = db.Column(db.String, nullable=False)
    register_time = db.Column(db.DateTime)
    role = db.Column(db.Enum(UserRole), default=UserRole.user)
