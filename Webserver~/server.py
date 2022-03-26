from SimpleWebSocketServer import SimpleWebSocketServer, WebSocket
import socket

# GLOBAL VARIABLES
clients = [] # number of clients connected to WebSocket Server

class Webserver(WebSocket):
    def handleMessage(self):
        for client in clients:
            if client != self:
                client.sendMessage(self.data)
            # ignore any weird character
            # socket_message = self.data.decode('unicode_escape').encode('ascii', 'ignore')
            #robot_task = self.process_hololens_data(socket_message)

    def handleConnected(self):
        print(self.address, 'connected')
        for client in clients:
            client.sendMessage(self.address[0] + u' - connected')
        clients.append(self)

    def handleClose(self):
        clients.remove(self)
        print(self.address, 'closed')
        for client in clients:
            client.sendMessage(self.address[0] + u' - disconnected')

def main():
    port_socket = 8080
    hostip = socket.gethostbyname("0.0.0.0") #get current computer's IP address
    print("Websocket format: ws://{}:{}/".format(hostip, port_socket))
    server = SimpleWebSocketServer(hostip, port_socket, Webserver)
    server.serveforever()

if __name__ == '__main__':
    main()