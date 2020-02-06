from pydantic import BaseModel


class FlavorPrice(BaseModel):
    id: int
    name: str
    vcpu: int
    memory_gb: int
    disk_gb: int
    price_per_min: int


