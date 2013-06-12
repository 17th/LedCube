cd Release
make all
avrdude.exe -c usbasp -p m16 -P usb -U flash:w:"LedCube00.hex":i