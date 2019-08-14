
float2 GetStencilUV(float2 uv)
{
  
  float2 stencilUV = uv;
  stencilUV.y = 1 - stencilUV.y;


  
  float bai = 1/1.62;// 9.0/12.0 * 0.8;//4;3 16;12 16;9
  stencilUV.y = stencilUV.y*bai + (1-bai)/2;
  
  return stencilUV;//x - y * floor(x / y);

}

