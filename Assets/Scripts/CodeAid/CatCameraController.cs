  
    public class CatCameraController
    {
        
        //震动相关
        private float _shakeDamage = 0;//shake代表损伤值，取值范围[0,1]

        public float shakeDamage
        {
            set
            {
                this._shakeDamage = value;
                if (this._shakeDamage>1f)
                {
                    this._shakeDamage = 1f;
                }

                if (this._shakeDamage<0f)
                {
                    this._shakeDamage = 0f;
                }
            }
            get
            {
                return this._shakeDamage;
            }
        }
        public CameraTempController _cameraControllerRC;
        //public float maxAngleX=10;
        public float maxAngleZ = 45;
        //public float maxAngleY = 10;
        public float maxoffsetX = 2f;
        public float maxoffsetZ = 2f;
        public float attenSpeed=0.03f;
        
        //偏移相关
        public float maxDisOffsetmin=10;//偏移允许最大距离,此时偏移最小
        public float minDisOffsetmax=5;//偏移到达最大值的距离.更近后偏移也不会增大

    }
