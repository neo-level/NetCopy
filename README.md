# NetCopy
Synchronous CLI application to send files over the network.

This is a command-line tool. You have to open a DOS/command prompt to run these. To run the program as a server (receiving a file), use the following command-line format. The filename is required. This is the destination filename that the file will be copied into.
$ ncp -p 9021 mypicture.png
The port option can be omitted. It defaults to the 9021 port if not specified:
$ ncp mypicture.png
To send a file, the following command-line format is used:
$ ncp -ip 127.0.0.1 -port 9021 mypicture.png
The IP is the IP address of the server. The port is the port number the service is running on. The last parameter is the local file to copy to the remote server.
