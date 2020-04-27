
/*
float2 GetStencilUV(float2 uv, float oy=0.002)
{
  
  float2 stencilUV = uv;
  stencilUV.y = 1 - stencilUV.y;

  float bai = 1/1.62;
  //float bai = 1/1.333333;

  float offsetY = (1-bai)/2+oy;
  stencilUV.y = stencilUV.y*bai + offsetY;

  return stencilUV;

}*/

float _GlobalStencilAspect;//
float _GlobalHorizonFlag;

float2 GetStencilUV( float2 uv ){//, float oy=0.002){

    float2 stencilUV = float2(
      1-uv.y,
      1-uv.x
    );

    if(_GlobalHorizonFlag==1){
      
      stencilUV.x = uv.x;
      stencilUV.y = 1-uv.y;

    }


    float bai = 1 / _GlobalStencilAspect;//1.62;
    //float bai = 1/1.333333;

    float offsetY = (1-bai)/2;//+oy;

    if(_GlobalHorizonFlag == 1){
      
      //横長
      //bai = 1 / bai;
      //offsetY = (1 - bai) / 2;
      stencilUV.y = stencilUV.y * bai + offsetY;

    }else{
      
      //縦長
      stencilUV.y = stencilUV.y * bai + offsetY;

    }

    return stencilUV;

}

