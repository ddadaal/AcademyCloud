from enum import Enum

from db import db
from db.models.utils import GUID


class BillingCycle(db.Model):
    """计费周期。
    """

    id = db.Column(GUID(), primary_key=True)


