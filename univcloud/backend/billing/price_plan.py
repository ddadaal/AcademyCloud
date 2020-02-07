from typing import Optional, Union, Tuple

from sqlalchemy import exc


# def get_user_by_username(username: str) -> Optional[User]:
#     return User.query.filter_by(username=username).first()
#
#
# # Returns User if success, None if conflict
# def add_user(username: str, password: str) -> Optional[User]:
#     # check uniqueness of username
#     if User.query.filter_by(username=username).first() is not None:
#         return None
#
#     user = User(username=username, password=password)
#
#     db.session.add(user)
#     db.session.commit()
#
#     return user
