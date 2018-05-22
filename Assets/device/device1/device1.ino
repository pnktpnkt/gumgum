#define ARM_STRETCH 6
#define GRAVITY_CHANGE 11

void setup() {
  // put your setup code here, to run once:
  pinMode(7,OUTPUT);  //皮膚引き伸ばし用　信号ピン
  pinMode(8,OUTPUT);
  pinMode(12,OUTPUT);  //重心移動用　信号ピン
  pinMode(13,OUTPUT);

  Serial.begin(9600);
  Serial.println("Start! input 2000");

}

int Serial_data(){ //シリアルで送るデータを3桁にするメソッド
  int i = 0;
  char c;
  char send_data[4];//モーターに送るデータ
  int motor_power;
  
  while(1){
    if(Serial.available() > 0){
        c = Serial.read();
        if(c >= '0' && c <= '9'){
        send_data[i] = c;
        i++;
        if(i == 4){
          send_data[i] = '\0';
          break;
        }
       }
       else if(i>0){
         send_data[i] = '\0';
         break;
       } 
    }  
  }
   motor_power = atoi(send_data);
   //Serial.println(motor_power);

   return motor_power;
}

/*モーターの回転方向、ブレーキのメソッド
シリアルから送られてきたデータは4桁として、それぞれこのようにする
・arm_stretch
    0000~0255 :前転
    0256~0511 :後転
    0600 : ブレーキ
    0700：フリー
・gravity_change
    1000~1255 :前転
    1256~1511 :後転
    1600 : ブレーキ
    1700 : フリー
*/
void arm_stretch_forward(){
  digitalWrite(7,HIGH);
  digitalWrite(8,LOW);
  Serial.println("arm_strech: forward");
}
void arm_stretch_backward(){
  digitalWrite(7,LOW);
  digitalWrite(8,HIGH);
  Serial.println("arm_strech: backward");
}
void arm_stretch_brake(){
  digitalWrite(7,HIGH);
  digitalWrite(8,HIGH);
  Serial.println("arm_strech: brake");
}
void arm_stretch_free(){
  digitalWrite(7,LOW);
  digitalWrite(8,LOW);
  Serial.println("arm_strech: free");
}

void gravity_change_forward(){
  digitalWrite(12,HIGH);
  digitalWrite(13,LOW);
  Serial.println("gravity_change: forward");
}
void gravity_change_backward(){
  digitalWrite(12,LOW);
  digitalWrite(13,HIGH);
  Serial.println("gravity_change: backward");
}
void gravity_change_brake(){
  digitalWrite(12,HIGH);
  digitalWrite(13,HIGH);
  Serial.println("gravity_change: brake");
}
void gravity_change_free(){
  digitalWrite(12,LOW);
  digitalWrite(13,LOW);
  Serial.println("gravity_change: free");
}

void setMotor(int device_number,int power,int delay_count){
  analogWrite(device_number,power);
  if(device_number == 6) Serial.print("Arm_Stretch : ");
  else Serial.print("Gravity_Change : ");
  Serial.println(power);
  delay(delay_count);
}


void loop() {
  int serial_data;
  int motor_power = 0;
  
  serial_data = Serial_data();
   
  if(serial_data == 2000){
    //初期状態
    gravity_change_brake();
    setMotor(GRAVITY_CHANGE,255,100);
    arm_stretch_free();
    setMotor(ARM_STRETCH,0,100);
  }
  else{
    //モーター駆動 
    if(serial_data / 1000 == 0){ //arm_strech 駆動
      motor_power = serial_data;
      if(0 <= motor_power && motor_power <= 255){
        arm_stretch_forward();
        setMotor(ARM_STRETCH,motor_power,100);
      }
      else if(255 < motor_power && motor_power <=511){
        motor_power %= 256;
        arm_stretch_backward();
        setMotor(ARM_STRETCH,motor_power,100);
      }
      else if(motor_power == 600){
        arm_stretch_brake();
        setMotor(ARM_STRETCH,255,100);
      }
      else{
        arm_stretch_free();
        setMotor(ARM_STRETCH,0,100);
      }
    }
    else{ //gravity_change 駆動
      motor_power = serial_data - 1000;
      if(0 <= motor_power && motor_power <= 255){
        gravity_change_forward();
        setMotor(GRAVITY_CHANGE,motor_power,100);
      }
      else if(255 <motor_power && motor_power <= 511){
        motor_power %= 256;
        gravity_change_backward();
        setMotor(GRAVITY_CHANGE,motor_power,100);
      }
      else if(motor_power == 600){
        gravity_change_brake();
        setMotor(GRAVITY_CHANGE,255,100);
      }
      else{
        gravity_change_free();
        setMotor(GRAVITY_CHANGE,0,100);
      }
    }
  }
}


