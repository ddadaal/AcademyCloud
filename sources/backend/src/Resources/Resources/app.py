import grpc
import services
from concurrent.futures import ThreadPoolExecutor

port = 50052

server = grpc.server(ThreadPoolExecutor(max_workers=10))
services.init_services(server)

print(f'Server running at localhost:{port}')

server.add_insecure_port(f'[::]:{port}')
server.start()
server.wait_for_termination()