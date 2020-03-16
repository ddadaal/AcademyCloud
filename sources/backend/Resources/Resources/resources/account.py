from flask import request
from flask_restx import Resource, fields, Namespace

ns = Namespace("account", description="登录注册")

scope = ns.model("范围", {
    "domainId": fields.String(help="域ID"),
    "projectId": fields.String(required=False, help="项目ID，可为None"),
    "role": fields.String(enum=["Admin", "Member"]),
})


@ns.route("login")
class AccountLoginResource(Resource):

    @ns.doc("获取用户可以登录的范围")
    @ns.expect(ns.model("ScopeRequest", {
        "username": fields.String(required=True, help="用户名"),
        "password": fields.String(required=True, help="密码")
    }))
    @ns.response(200, "用户名和密码有效，返回用户可以登录的范围", ns.model("Scopes Response", {
        "scopes": fields.List(fields.Nested(ns.inherit("ScopeInfo", scope, {
            "domainName": fields.String(help="域名字"),
            "projectName": fields.String(required=False, help="项目名字"),
        })))
    }))
    @ns.response(401, "用户名和密码无效。")
    def get(self):
        args = request.get_json()
        return

    @ns.doc("使用用户指定的用户名、密码和范围登录")
    @ns.expect(ns.model("LoginRequest", {
        "username": fields.String(required=True, help="用户名"),
        "password": fields.String(required=True, help="密码"),
        "scope": fields.Nested(scope, required=True, help="以登录的范围"),
    }))
    @ns.response(200, "用户名、密码和范围有效，返回token。", {
        "token": fields.String(help="Token")
    })
    @ns.response(401, "用户名、密码或者范围无效。")
    def post(self):
        args = request.get_json()
        return


@ns.route("register")
class AccountRegisterResource(Resource):
    @ns.doc("注册")
    @ns.expect(ns.model("RegisterRequest", {
        "username": fields.String(required=True, help="用户名"),
        "password": fields.String(required=True, help="密码"),
    }))
    @ns.response(200, "注册成功")
    def post(self):
        pass
