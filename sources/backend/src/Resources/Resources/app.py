import grpc
import services
from concurrent.futures import ThreadPoolExecutor

server = grpc.server(ThreadPoolExecutor(max_workers=10))
services.init_services(server)

server.add_insecure_port('[::]:50051')
server.start()
server.wait_for_termination()