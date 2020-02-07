from enum import Enum

from flask_sqlalchemy import SQLAlchemy

db = SQLAlchemy()


class PricePlan(db.Model):
    """
    价格计划
    价格计划不能删除不能修改
    要修改需要重新新建一个新的price_plan.
    """
    __tablename__ = "price_plan"

    id = db.Column(db.Integer, primary_key=True)
    name = db.Column(db.String, nullable=True)
    create_time = db.Column(db.DateTime)
    # 价格以分记，每个单位每个小时的价格
    vcpu = db.Column(db.Integer)
    memory_gb = db.Column(db.Integer)
    disk_gb = db.Column(db.Integer)
    floating_ip = db.Column(db.Integer)
    bandwidth = db.Column(db.Integer)


class AssignmentAllocationTarget(Enum):
    Domain = 1,
    Project = 2


class PricePlanAssignment(db.Model):
    """
    价格计划的分配
    若end_time为空，则为持续到现在。
    每次价格计划修改，都应该将当前的计划的end_time设置后，并建立一条新的记录。
    """
    __tablename__ = "price_plan_assignment"
    id = db.Column(db.Integer, primary_key=True)

    target_type = db.Column(db.Enum(AssignmentAllocationTarget), index=True)
    target_id = db.Column(db.String, index=True)
    start_time = db.Column(db.DateTime, index=True)
    end_time = db.Column(db.DateTime, index=True, nullable=True)
    plan_id = db.Column(db.Integer, index=True)


class ResourceAllocation(db.Model):
    __tablename__ = "resource_allocation"

    id = db.Column(db.Integer, primary_key=True)

    target_type = db.Column(db.Enum(AssignmentAllocationTarget), index=True)
    target_id = db.Column(db.String, index=True)
    start_time = db.Column(db.DateTime, index=True)
    end_time = db.Column(db.DateTime, index=True, nullable=True)

    vcpu = db.Column(db.Integer)
    memory_gb = db.Column(db.Integer)
    disk_gb = db.Column(db.Integer)
    floating_ip = db.Column(db.Integer)
    bandwidth = db.Column(db.Integer)


class UserLimitation(db.Model):
    __tablename__ = "user_limitation"

    id = db.Column(db.Integer, primary_key=True)

    user_id = db.Column(db.String, index=True)
    start_time = db.Column(db.DateTime, index=True)
    end_time = db.Column(db.DateTime, index=True, nullable=True)

    vcpu = db.Column(db.Integer)
    memory_gb = db.Column(db.Integer)
    disk_gb = db.Column(db.Integer)
    floating_ip = db.Column(db.Integer)
    bandwidth = db.Column(db.Integer)
