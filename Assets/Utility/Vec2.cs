

using Newtonsoft.Json;
using UnityEngine;

namespace W
{

    [JsonObject(MemberSerialization.OptIn)]
    public struct Vec2
    {
        public static Vec2 invalid => new Vec2(int.MinValue, int.MinValue);
        public static Vec2 zero => new Vec2(0, 0);
        public static Vec2 one => new Vec2(1, 1);
        public static Vec2 left => new Vec2(-1, 0);
        public static Vec2 right => new Vec2(1, 0);
        public static Vec2 down => new Vec2(0, -1);
        public static Vec2 up => new Vec2(0, 1);

        public static Vec2 left_up => new Vec2(-1, 1);
        public static Vec2 right_up => new Vec2(1, 1);
        public static Vec2 left_down => new Vec2(-1, -1);
        public static Vec2 right_down => new Vec2(1, -1);

        public float Magnitude => M.Sqrt(SqrMagnitude);
        public int SqrMagnitude => x * x + y * y;


        public static Vec2 Create(int x, int y) {
            return new Vec2(x, y);
        }

        public Vec2(uint x, uint y) {
            this.x = (int)x;
            this.y = (int)y;
        }
        public Vec2(int x, int y) {
            this.x = x;
            this.y = y;
        }
        [JsonProperty]
        public int x;
        [JsonProperty]
        public int y;


        public override string ToString() {
            return $"({x}, {y})";
        }

        public static int CornerDistanceBewtween(Vec2 lhs, Vec2 rhs) => M.Abs(lhs.x - rhs.x) + M.Abs(lhs.y - rhs.y);



        public static implicit operator Vec2(Vector2Int vec) {
            return new Vec2(vec.x, vec.y);
        }
        public static implicit operator Vec2(Vector3Int vec) {
            return new Vec2(vec.x, vec.y);
        }

        public static implicit operator Vector2Int(Vec2 pos) {
            return new Vector2Int(pos.x, pos.y);
        }

        public static implicit operator Vector2(Vec2 pos) {
            return new Vector2(pos.x, pos.y);
        }
        public static implicit operator Vector3(Vec2 pos) {
            return new Vector3(pos.x, pos.y, 0);
        }

        public static Vec2 operator +(Vec2 lhs, Vec2 rhs) {
            return new Vec2(lhs.x + rhs.x, lhs.y + rhs.y);
        }
        public static Vec2 operator -(Vec2 lhs, Vec2 rhs) {
            return new Vec2(lhs.x - rhs.x, lhs.y - rhs.y);
        }

        public static bool operator ==(Vec2 lhs, Vec2 rhs) {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }
        public static bool operator !=(Vec2 lhs, Vec2 rhs) {
            return lhs.x != rhs.x || lhs.y != rhs.y;
        }

        public static Vec2 operator *(int lhs, Vec2 rhs) {
            return new Vec2(lhs * rhs.x, lhs * rhs.y);
        }
        public static Vec2 operator *(Vec2 lhs, int rhs) {
            return new Vec2(lhs.x * rhs, lhs.y * rhs);
        }

        public static Vec2 operator /(int lhs, Vec2 rhs) {
            return new Vec2(lhs / rhs.x, lhs / rhs.y);
        }
        public static Vec2 operator /(Vec2 lhs, int rhs) {
            return new Vec2(lhs.x / rhs, lhs.x / rhs);
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj is Vec2) {
                Vec2 vec2 = (Vec2)obj;
                return this == vec2;
            }
            return false;
        }
        public override int GetHashCode() {
            return x.GetHashCode() + y.GetHashCode();
        }



    }




    [JsonObject(MemberSerialization.OptIn)]
    public struct Vec3
    {
        public Vec3(int x = 0, int y = 0, int z = 0) {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        [JsonProperty]
        public int x;
        [JsonProperty]
        public int y;
        [JsonProperty]
        public int z;

        public static implicit operator Vec3(Vector3Int vec) {
            return new Vector3Int(vec.x, vec.y, vec.z);
        }

        public static implicit operator Vector3Int(Vec3 pos) {
            return new Vector3Int(pos.x, pos.y, pos.z);
        }

        public static explicit operator Vector3(Vec3 pos) {
            return new Vector3(pos.x, pos.y, pos.z);
        }

        public static Vec3 operator +(Vec3 lhs, Vec3 rhs) {
            return new Vec3(lhs.x + rhs.x, lhs.y + rhs.y, lhs.z + rhs.z);
        }
        public static Vec3 operator -(Vec3 lhs, Vec3 rhs) {
            return new Vec3(lhs.x - rhs.x, lhs.y - rhs.y, lhs.z - rhs.z);
        }

        public static bool operator ==(Vec3 lhs, Vec3 rhs) {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }
        public static bool operator !=(Vec3 lhs, Vec3 rhs) {
            return !(lhs == rhs);
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj is Vec3) {
                Vec3 vec = (Vec3)obj;
                return this == vec;
            }
            return false;
        }
        public override int GetHashCode() {
            return x.GetHashCode() + y.GetHashCode() + z.GetHashCode();
        }

        public static Vec3 zero => new Vec3(0, 0, 0);
        public static Vec3 one => new Vec3(1, 1, 1);
        public static Vec3 left => new Vec3(-1, 0, 0);
        public static Vec3 right => new Vec3(1, 0, 0);
        public static Vec3 down => new Vec3(0, -1, 0);
        public static Vec3 up => new Vec3(0, 1, 0);
        public static Vec3 forward => new Vec3(0, 0, -1);
        public static Vec3 back => new Vec3(0, 0, 1);

    }
}
