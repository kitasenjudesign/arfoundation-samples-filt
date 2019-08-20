float4 GetFullScreenVert(float4 vertex){

    vertex.x *= 2.0;
    vertex.y *= 2.0;
    vertex.z = 0.00001;
    vertex.w = 1.0;

    return vertex;
}