
void setup(){
}

void loop(){
  spin(0);

  delay(200);

  spin(1);
}

void spin(int d){
  int o1, o2;
  //モーターの強さ指定
  int val = 1; //0~255の値にする
  if(d == 0){
    o1 = 3;
    o2 = 5;
  }else{
    o1 = 5;
    o2 = 3;
  }

  digitalWrite(o1 , LOW);
  //valが大きいほど出力値も大きくなる
  for(val = 1; val < 255 ; val++){
    analogWrite(o2, val); //出力値:1~255
    delay(5);
  }
}




