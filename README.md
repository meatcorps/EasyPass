# EasyPass
Easy to use password dongle. That will behave as a HID keyboard device. Then after flashing it will automatically send the password. The total price for DIY will be around $3!

# Requirements
This project require 3 things. 
 - PC C# application to flash the password to the device. There is a GUI or Console application.
 - USB device. This is chinese cheap WCH CH552g core board. Including a USB A port. For example: https://www.aliexpress.com/item/1005005265280621.html
   
   ![image](https://github.com/meatcorps/EasyPass/assets/32952469/9a3640a4-8059-4e32-86f2-d56570faf521)
   
 - Firmware which is a based on beautiful work of DeqingSun ch55xduino.

# Getting started

It require some work. Bare with me! I suggest the use the Easy ways. The other option is when you like to hack your self or are interesting to make something else with it.

## Hardware

Order the products. I advice the order multiple. Because it can happen that one is not working. Ow I also advise for finishing touch to order or reuse a USB key like this:

![image](https://github.com/meatcorps/EasyPass/assets/32952469/99cfbf99-0b13-4a42-9087-96ecd74bea04)

Those will fit nicely and will protect the device.

## Hardware Firmware

### Easy way (Requimented)
 - (If you not already have it) Download the WCHIPSStudio utility from the original vendor WCH. https://www.wch-ic.com/downloads/WCHISPTool_Setup_exe.html
 - Open the tool. Click on E8051 USB MCUs.
 - Insert the dongle in download mode. (2 pins in the middle)

![image](https://github.com/meatcorps/EasyPass/assets/32952469/404d344a-03ca-4018-8045-0b0277b097fa)

- 1 Check if the values are the same as in the screenshot. If the device is correctly detected it will show op as CH55x---X1 device
- 2 Download the easypass.hex file from https://github.com/meatcorps/EasyPass/releases/ and select it by pressing the ...
- 3 Select all the options
- 4 Start flashing or like they like to say download...
- Remove the device and close the WCH util.
  
### Hard development way:

First we need to get the firmware to the WCH CH552 device. For this you will need to follow the guide created by DeqingSun. https://github.com/DeqingSun/ch55xduino. This also require the Arduino IDE.

Check out the repo. Open the ino file within the Firmware/Macroboard folder.

After you followed the guide, open the arduino project in Firmware/macroboard. Set the following: 
 - Tools > Board to "CH552 Board".
 - Tools > USB settings to USER CODE w/ 266B USB ram
 - Verify the code. Should be ok. If not please open an issue.
 - Insert the device and start upload within a few seconds. Otherwise it will fail! 
 - Remove the device 

## Software

### Easy way

Just download the easypassgui.zip from https://github.com/meatcorps/EasyPass/releases/. Unzip and run EasyPassFlasher.exe. Then you should see something like this:

![image](https://github.com/meatcorps/EasyPass/assets/32952469/d31ffa67-df1f-4d25-a628-830b0d0f59a2)

Insert the dongle (NOT IN DOWNLOAD MODE!) 

The color below should go from red to green. Then type or copy paste the password press the flash button. Keep in mind it work only once after insert within the USB port. If you want to re-flash. Please re-insert the dongle and restart the application.

### Hard way (really? sure)

Checkout the complete repo. Open it in your favorite C# IDE. Go nuts...

