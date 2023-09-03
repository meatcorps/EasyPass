/*
    Toby's Low Cost Macro Keyboard
*/

#ifndef USER_USB_RAM
#error "Require USB RAM. Go Tools > USB Setting and pick the 2nd option in the dropdown list"
#endif

#include "src/userUsbHidMediaKeyboard/USBHIDMediaKeyboard.h"
#include "src/userUsbHidMediaKeyboard/USBhandler.h"
#include "win-zh_util.h"

////////////// HARDWARE CONFIG //////////

uint8_t run = 1;
uint8_t led = 0;

void setup() {
  USBInit();
  pinMode(30, OUTPUT);
}

void loop() {
  LedOff();
  delay(1000);

  if (run == 1) {
    if (eeprom_read_byte(0) == 128) {
      for (uint8_t i = 0; i < 30; i++) {
        uint8_t eepromData = eeprom_read_byte(i);
        
        if (eepromData > 2) {
          Keyboard_write(eepromData);
        }
        delay(50);
      }
    }

    run = 0;
    LedOn();
    delay(3000);  //naive debouncing
  } else {
    if (raw_hid_received() == 1 && Ep2Buffer[0] == 0x45 && Ep2Buffer[1] == 0x46) // Some random numbers
    {
      if (led == 1) {
        LedOff();
      } else {
        LedOn();
      }
      
      eeprom_write_byte(0, 128);
      for (uint8_t i = 2; i < 32; i++) {
        Ep2Buffer[64 + i] = Ep2Buffer[i];
        eeprom_write_byte(i - 1, Ep2Buffer[i]);
      }
      
      Ep2Buffer[0] = 0;
      Ep2Buffer[1] = 0;

      USB_EP2_send();
    }
    delay(50);  //naive debouncing
  }

  
}

void LedOn() {
  led = 1;
  digitalWrite(30, HIGH);
}

void LedOff() {
  led = 0;

  digitalWrite(30, LOW);
}
