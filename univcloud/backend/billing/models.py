from sqlalchemy import Boolean, Column, ForeignKey, Integer, String, Float, DateTime
from sqlalchemy.orm import relationship

from .database import Base


class FlavorPrice(Base):
    __tablename__ = "flavor_prices"

    id = Column(Integer, primary_key=True, index=True)
    flavor_id = Column(str, index=True)
    name = Column(String)
    vcpu = Column(Integer)
    memory_gb = Column(Integer)
    disk_gb = Column(Integer)
    price_per_min = Column(Integer)


class FlavorChangePrice(Base):
    __tablename__ = "flavor_change_price"
    id = Column(Integer, primary_key=True, index=True)
    before = Column(Integer)
    after = Column(Integer)
    time = Column(DateTime)


class Usage(Base):
    __tablename__ = "usages"

    id = Column(Integer, primary_key=True, index=True)
    flavor_id = Column(str, index=True)
    start = Column(DateTime)
    end = Column(DateTime)
