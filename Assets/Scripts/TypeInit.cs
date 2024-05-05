using System;
using UnityEngine;

public class TypeInit : MonoBehaviour
{
    public struct IntVector2
    {
        public int x, y;

        public IntVector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator==(IntVector2 object_, IntVector2 other)
        {
            if(object_.x == other.x && object_.y == other.y) return true;
            return false;
        }

        public static bool operator!=(IntVector2 object_, IntVector2 other)
        {
            if (object_.x != other.x || object_.y != other.y) return true;
            return false;
        }

        public static implicit operator IntVector2(Vector2 other)
        {
            IntVector2 iv2 = new IntVector2((int)other.x, (int)other.y);
            return iv2;
        }

        public static IntVector2 operator+(IntVector2 object_, IntVector2 other)
        {
            return new IntVector2(object_.x + other.x, object_.y + other.y);
        }

        public override bool Equals(object obj)
        {
            return obj is IntVector2 vector &&
                   x == vector.x &&
                   y == vector.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public override string ToString()
        {
            return x.ToString() + ":" + y.ToString();
        }
    }

    [Serializable]
    public struct DialogLine
    {
        public string sender;
        public string message;
        public AudioClip audioClip;

        public DialogLine(string sender, string message, AudioClip audioClip)
        {
            this.sender = sender;
            this.message = message;
            this.audioClip = audioClip;
        }
    }

    [Serializable]
    public struct WallsFilled
    {
        public bool N;
        public bool E;
        public bool S;
        public bool W;

        public override string ToString()
        {
            return "Northern Wall: " + N + "; Eastern Wall: " + E + "; Southern Wall: " + S + "; Western Wall: " + W;
        }
    }
}
