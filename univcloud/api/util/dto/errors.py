from typing import TypedDict, List


class ErrorDto(TypedDict):
    code: str


class StandardErrorDto(ErrorDto):
    description: str


class MultipleErrorsDto(ErrorDto):
    descriptions: List[str]
