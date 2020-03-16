def init_services(server):
    import services.identity
    identity.add_to_server(server)