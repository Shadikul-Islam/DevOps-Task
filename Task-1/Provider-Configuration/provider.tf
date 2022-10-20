provider "aws" {
  profile    = "default"
  region     = "us-east-1"
}

resource "aws_instance" "default" {
  ami           = "ami-2551f530"
  instance_type = "c3.4xlarge"
}