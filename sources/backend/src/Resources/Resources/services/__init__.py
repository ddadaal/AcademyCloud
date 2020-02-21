def init_services(server):
    import services.user_management
    user_management.add_to_server(server)