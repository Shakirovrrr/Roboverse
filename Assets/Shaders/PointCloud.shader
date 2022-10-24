Shader "Custom/PointCloud"
{
 
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
 
        Pass
        {
            Name "Default"
            Tags{ "LightMode" = "Always" }
 
            HLSLPROGRAM
 
            #pragma raytracing test
 
            struct RayPayload
            {
                float4 color;
            };

            struct RayAttributes
            {
                float2 barycentrics;
            };
 
            [shader("closesthit")]
            void ClosestHit(inout RayPayload payload : SV_RayPayload, RayAttributes attributes : SV_IntersectionAttributes)
            {
                float3 worldRayOrigin = WorldRayOrigin() + WorldRayDirection() * RayTCurrent();
                payload.color = float4(worldRayOrigin, 1);
            }
 
            ENDHLSL
        }
    }
}
