# Use the official Ubuntu as a base image
FROM ubuntu:latest

# Install curl and telnet
RUN apt-get update && apt-get install -y curl telnet

# Clean up package cache to reduce image size
RUN apt-get clean && rm -rf /var/lib/apt/lists/*

# Set the default command to start a shell when the container runs
CMD ["/bin/bash"]
