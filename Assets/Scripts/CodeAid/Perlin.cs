using Unity.Mathematics;

public static class Perlin
{
    /// <summary>
    /// 0-1,越高细节越复杂
    /// </summary>
    public static float persistence;//持续度,振幅?,
    /// <summary>
    /// 整数倍 倍频,细节噪声缩几倍于主噪声线
    /// </summary>
    public static int Number_Of_Octaves;//频率加倍数
    
    public static int Seed=0;
    static Perlin()
    {
        persistence = 0.5f;//设置强弱细节表现
        Number_Of_Octaves = 4;//细节密度
    }
    
    
    public static double Noise(int x, int y,int _Seed)    // 根据(x,y)获取一个初步噪声值,随机器
    {
        int n = x + y * 57+_Seed*7;
        n = (n << 13) ^ n;//^相反为0,相同为1,<<移位
        return (1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);//这里用的全是质数,除了自身和1没因数
    }

    public static double SmoothedNoise(int x, int y,int _Seed)   //光滑噪声
    {
        double corners = (Noise(x - 1, y - 1,_Seed) + Noise(x + 1, y - 1,_Seed) + Noise(x - 1, y + 1,_Seed) + Noise(x + 1, y + 1,_Seed)) / 16;
        double sides = (Noise(x - 1, y,_Seed) + Noise(x + 1, y,_Seed) + Noise(x, y - 1,_Seed) + Noise(x, y + 1,_Seed)) / 8;//贡献1/2权重
        double center = Noise(x, y,_Seed) / 4;//1/4权重
        return corners + sides + center;
    }
    public static double Cosine_Interpolate(double a, double b, double x)  // 余弦插值
    {
        double ft = x * 3.1415927;
        double f = (1 - math.cos(ft)) * 0.5;
        return a * (1 - f) + b * f;
    }
    public static double Linear_Interpolate(double a, double b, double x) //线性插值
    {
        return a * (1 - x) + b * x;
    }

    public static double InterpolatedNoise(float x, float y,int _Seed)   // 获取插值噪声
    {
        int integer_X = (int)x;
        float fractional_X = x - integer_X;
        int integer_Y = (int)y;
        float fractional_Y = y - integer_Y;
        double v1 = SmoothedNoise(integer_X, integer_Y,_Seed);
        double v2 = SmoothedNoise(integer_X + 1, integer_Y,_Seed);
        double v3 = SmoothedNoise(integer_X, integer_Y + 1,_Seed);
        double v4 = SmoothedNoise(integer_X + 1, integer_Y + 1,_Seed);
        double i1 = Cosine_Interpolate(v1, v2, fractional_X);
        double i2 = Cosine_Interpolate(v3, v4, fractional_X);
        return Cosine_Interpolate(i1, i2, fractional_Y);
    }

    public static double PerlinNoise(float x, float y)    // 最终调用：根据(x,y)获得其对应的PerlinNoise值
    {
        double noise = 0;
        double p = persistence;
        int n = Number_Of_Octaves;
        for (int i = 0; i < n; i++)
        {
            double frequency = math.pow(2, i);
            double amplitude = math.pow(p, i);
            noise = noise + InterpolatedNoise((float)(x * frequency), (float)(y * frequency),Seed) * amplitude;
        }

        return noise;
    }

    /// <summary>
    /// 返0-1值,x,y是取样,
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="_Seed"></param>
    /// <param name="_persistence"></param>
    /// <param name="_Number_Of_Octaves"></param>
    /// <returns></returns>
    public static double PerlinNoiseSeedCustom(float x, float y,int _Seed,double _persistence,int _Number_Of_Octaves)    // 最终调用：根据(x,y)获得其对应的PerlinNoise值
    {
        double noise = 0;
        double p = _persistence;
        int n = _Number_Of_Octaves;
        for (int i = 0; i < n; i++)
        {
            double frequency = math.pow(2, i);
            double amplitude = math.pow(p, i);
            noise = noise + InterpolatedNoise((float)(x * frequency), (float)(y * frequency),_Seed) * amplitude;
        }

        return noise;
    }
    }
