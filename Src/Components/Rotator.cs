namespace Toryngine;

public class Rotator : Component
{
    bool ret = false;
    public float speed = 0.006f;
    
    public override void Update()
    {
        base.Update();

        if (!ret)
        {
            if (owner!.transform.Scale.X > -1)
            {
                owner.transform.Scale.X -= speed;
            }
            else ret = true;
        }
        else
        {
            if (owner!.transform.Scale.X < 1)
            {
                owner.transform.Scale.X += speed;
            }
            else ret = false;
        }
    }
}