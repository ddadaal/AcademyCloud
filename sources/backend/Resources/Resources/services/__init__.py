def init_services(server):
    import services.identity
    identity.add_to_server(server)

    import services.instance
    instance.add_to_server(server)