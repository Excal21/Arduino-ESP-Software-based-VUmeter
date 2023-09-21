byte LED[8] = {D0, D1, D2, D3, D5, D6, D7, D8};

char incomingByte = 0;
int data = 0;

void DisplayVal(int val = 0)
{
  for (int i = 0; i < 8; i++)
  {
    digitalWrite(LED[i], ((val / 10) > i) ? 1 : 0);
  }
}

void setup()
{
  // put your setup code here, to run once:
  Serial.begin(9600);
  Serial.println("Booted up");
  for (size_t i = 0; i < 8; i++)
  {
    pinMode(LED[i], OUTPUT);
  }

  delay(1000);

  for (size_t i = 0; i < 8; i++)
  {
    digitalWrite(LED[i], 1);
    delay(80);
  }
  for (size_t i = 0; i < 8; i++)
  {
    digitalWrite(LED[i], 0);
    delay(80);
  }
}

void loop()
{
  if (Serial.available() > 0)
  {
    data = Serial.parseInt();
    if (data == 1000)
    {
      for (size_t i = 0; i < 8; i++)
      {
        digitalWrite(LED[i], 1);
        delay(80);
      }
      for (size_t i = 0; i < 8; i++)
      {
        digitalWrite(LED[i], 0);
        delay(80);
      }
    }
    else
    {

      DisplayVal(data);
    }
  }
}