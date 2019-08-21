
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

float2 GetStencilUV(float2 uv, float oy=0.002){

    float2 stencilUV = float2(
      1-uv.y,
      1-uv.x
    );

    float bai = 1/1.62;
    //float bai = 1/1.333333;

    float offsetY = (1-bai)/2+oy;
    stencilUV.y = stencilUV.y*bai + offsetY;

    return stencilUV;

}

