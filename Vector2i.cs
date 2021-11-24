﻿#if GODOT
using Godot;
#elif UNITY_5_3_OR_NEWER
using UnityEngine;
#endif
using System;
using System.Runtime.InteropServices;

#if GODOT_REAL_T_IS_DOUBLE
using real_t = System.Double;
#else
using real_t = System.Single;
#endif

namespace ExtraMath
{
    /// <summary>
    /// 2-element structure that can be used to represent 2D grid coordinates or pairs of integers.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2I : IEquatable<Vector2I>
    {
        /// <summary>
        /// Enumerated index values for the axes.
        /// Returned by <see cref="MaxAxis"/> and <see cref="MinAxis"/>.
        /// </summary>
        public enum Axis
        {
            X = 0,
            Y
        }

        /// <summary>
        /// The vector's X component. Also accessible by using the index position `[0]`.
        /// </summary>
        public int x;
        /// <summary>
        /// The vector's Y component. Also accessible by using the index position `[1]`.
        /// </summary>
        public int y;

        /// <summary>
        /// Access vector components using their index.
        /// </summary>
        /// <value>`[0]` is equivalent to `.x`, `[1]` is equivalent to `.y`.</value>
        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return x;
                    case 1:
                        return y;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        x = value;
                        return;
                    case 1:
                        y = value;
                        return;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        /// <summary>
        /// Returns a new vector with all components in absolute values (i.e. positive).
        /// </summary>
        /// <returns>A vector with <see cref="(float)Math.Abs(int)"/> called on each component.</returns>
        public Vector2I Abs()
        {
            return new Vector2I(Math.Abs(x), Math.Abs(y));
        }

        /// <summary>
        /// Returns this vector's angle with respect to the X axis, or (1, 0) vector, in radians.
        ///
        /// Equivalent to the result of <see cref="(float)Math.Atan2(real_t, real_t)"/> when
        /// called with the vector's `y` and `x` as parameters: `(float)Math.Atan2(v.y, v.x)`.
        /// </summary>
        /// <returns>The angle of this vector, in radians.</returns>
        public real_t Angle()
        {
            return (float)Math.Atan2(y, x);
        }

        /// <summary>
        /// Returns the angle to the given vector, in radians.
        /// </summary>
        /// <param name="to">The other vector to compare this vector to.</param>
        /// <returns>The angle between the two vectors, in radians.</returns>
        public real_t AngleTo(Vector2I to)
        {
            return (float)Math.Atan2(Cross(to), Dot(to));
        }

        /// <summary>
        /// Returns the angle between the line connecting the two points and the X axis, in radians.
        /// </summary>
        /// <param name="to">The other vector to compare this vector to.</param>
        /// <returns>The angle between the two vectors, in radians.</returns>
        public real_t AngleToPoint(Vector2I to)
        {
            return (float)Math.Atan2(y - to.y, x - to.x);
        }

        /// <summary>
        /// Returns the aspect ratio of this vector, the ratio of `x` to `y`.
        /// </summary>
        /// <returns>The `x` component divided by the `y` component.</returns>
        public real_t Aspect()
        {
            return x / (real_t)y;
        }

        /// <summary>
        /// Returns the cross product of this vector and `b`.
        /// </summary>
        /// <param name="b">The other vector.</param>
        /// <returns>The cross product vector.</returns>
        public int Cross(Vector2I b)
        {
            return x * b.y - y * b.x;
        }

        /// <summary>
        /// Returns the squared distance between this vector and `b`.
        /// This method runs faster than <see cref="DistanceTo"/>, so prefer it if
        /// you need to compare vectors or need the squared distance for some formula.
        /// </summary>
        /// <param name="b">The other vector to use.</param>
        /// <returns>The squared distance between the two vectors.</returns>
        public int DistanceSquaredTo(Vector2I b)
        {
            return (b - this).LengthSquared();
        }

        /// <summary>
        /// Returns the distance between this vector and `b`.
        /// </summary>
        /// <param name="b">The other vector to use.</param>
        /// <returns>The distance between the two vectors.</returns>
        public real_t DistanceTo(Vector2I b)
        {
            return (b - this).Length();
        }

        /// <summary>
        /// Returns the dot product of this vector and `b`.
        /// </summary>
        /// <param name="b">The other vector to use.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public int Dot(Vector2I b)
        {
            return x * b.x + y * b.y;
        }

        /// <summary>
        /// Returns the length (magnitude) of this vector.
        /// </summary>
        /// <returns>The length of this vector.</returns>
        public real_t Length()
        {
            int x2 = x * x;
            int y2 = y * y;

            return (float)Math.Sqrt(x2 + y2);
        }

        /// <summary>
        /// Returns the squared length (squared magnitude) of this vector.
        /// This method runs faster than <see cref="Length"/>, so prefer it if
        /// you need to compare vectors or need the squared length for some formula.
        /// </summary>
        /// <returns>The squared length of this vector.</returns>
        public int LengthSquared()
        {
            int x2 = x * x;
            int y2 = y * y;

            return x2 + y2;
        }

        /// <summary>
        /// Returns the axis of the vector's largest value. See <see cref="Axis"/>.
        /// If both components are equal, this method returns <see cref="Axis.X"/>.
        /// </summary>
        /// <returns>The index of the largest axis.</returns>
        public Axis MaxAxis()
        {
            return x < y ? Axis.Y : Axis.X;
        }

        /// <summary>
        /// Returns the axis of the vector's smallest value. See <see cref="Axis"/>.
        /// If both components are equal, this method returns <see cref="Axis.Y"/>.
        /// </summary>
        /// <returns>The index of the smallest axis.</returns>
        public Axis MinAxis()
        {
            return x < y ? Axis.X : Axis.Y;
        }

#if GODOT
        /// <summary>
        /// Returns a vector composed of the <see cref="(float)Math.PosMod(int, int)"/> of this vector's components and `mod`.
        /// </summary>
        /// <param name="mod">A value representing the divisor of the operation.</param>
        /// <returns>A vector with each component <see cref="(float)Math.PosMod(int, int)"/> by `mod`.</returns>
        public Vector2I PosMod(int mod)
        {
            Vector2I v = this;
            v.x = (float)Math.PosMod(v.x, mod);
            v.y = (float)Math.PosMod(v.y, mod);
            return v;
        }

        /// <summary>
        /// Returns a vector composed of the <see cref="(float)Math.PosMod(int, int)"/> of this vector's components and `modv`'s components.
        /// </summary>
        /// <param name="modv">A vector representing the divisors of the operation.</param>
        /// <returns>A vector with each component <see cref="(float)Math.PosMod(int, int)"/> by `modv`'s components.</returns>
        public Vector2I PosMod(Vector2I modv)
        {
            Vector2I v = this;
            v.x = (float)Math.PosMod(v.x, modv.x);
            v.y = (float)Math.PosMod(v.y, modv.y);
            return v;
        }
#endif // GODOT

        /// <summary>
        /// Returns a vector with each component set to one or negative one, depending
        /// on the signs of this vector's components, or zero if the component is zero,
        /// by calling <see cref="(float)Math.Sign(int)"/> on each component.
        /// </summary>
        /// <returns>A vector with all components as either `1`, `-1`, or `0`.</returns>
        public Vector2I Sign()
        {
            Vector2I v = this;
#if GODOT
            v.x = (float)Math.Sign(v.x);
            v.y = (float)Math.Sign(v.y);
#else
            v.x = v.x < 0 ? -1 : 1;
            v.y = v.y < 0 ? -1 : 1;
#endif
            return v;
        }

        /// <summary>
        /// Returns a perpendicular vector rotated 90 degrees counter-clockwise
        /// compared to the original, with the same length.
        /// </summary>
        /// <returns>The perpendicular vector.</returns>
        public Vector2I Orthogonal()
        {
            return new Vector2I(y, -x);
        }

        // Constants
        private static readonly Vector2I _zero = new Vector2I(0, 0);
        private static readonly Vector2I _one = new Vector2I(1, 1);

        private static readonly Vector2I _up = new Vector2I(0, -1);
        private static readonly Vector2I _down = new Vector2I(0, 1);
        private static readonly Vector2I _right = new Vector2I(1, 0);
        private static readonly Vector2I _left = new Vector2I(-1, 0);

        /// <summary>
        /// Zero vector, a vector with all components set to `0`.
        /// </summary>
        /// <value>Equivalent to `new Vector2I(0, 0)`</value>
        public static Vector2I Zero { get { return _zero; } }
        /// <summary>
        /// One vector, a vector with all components set to `1`.
        /// </summary>
        /// <value>Equivalent to `new Vector2I(1, 1)`</value>
        public static Vector2I One { get { return _one; } }

        /// <summary>
        /// Up unit vector. Y is down in 2D, so this vector points -Y.
        /// </summary>
        /// <value>Equivalent to `new Vector2I(0, -1)`</value>
        public static Vector2I Up { get { return _up; } }
        /// <summary>
        /// Down unit vector. Y is down in 2D, so this vector points +Y.
        /// </summary>
        /// <value>Equivalent to `new Vector2I(0, 1)`</value>
        public static Vector2I Down { get { return _down; } }
        /// <summary>
        /// Right unit vector. Represents the direction of right.
        /// </summary>
        /// <value>Equivalent to `new Vector2I(1, 0)`</value>
        public static Vector2I Right { get { return _right; } }
        /// <summary>
        /// Left unit vector. Represents the direction of left.
        /// </summary>
        /// <value>Equivalent to `new Vector2I(-1, 0)`</value>
        public static Vector2I Left { get { return _left; } }

        public static Vector2I[] GetNeighbouringPositions(Vector2I pos)
        {
            return new Vector2I[] { pos + Down, pos + Right, pos + Up, pos + Left };
        }
        public Vector2I[] Neighbours
        {
            get => GetNeighbouringPositions(this);
        }

        /// <summary>
        /// Constructs a new <see cref="Vector2I"/> with the given components.
        /// </summary>
        /// <param name="x">The vector's X component.</param>
        /// <param name="y">The vector's Y component.</param>
        public Vector2I(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructs a new <see cref="Vector2I"/> from an existing <see cref="Vector2I"/>.
        /// </summary>
        /// <param name="vi">The existing <see cref="Vector2I"/>.</param>
        public Vector2I(Vector2I vi)
        {
            this.x = vi.x;
            this.y = vi.y;
        }

        public static Vector2I operator +(Vector2I left, Vector2I right)
        {
            left.x += right.x;
            left.y += right.y;
            return left;
        }

        public static Vector2I operator -(Vector2I left, Vector2I right)
        {
            left.x -= right.x;
            left.y -= right.y;
            return left;
        }

        public static Vector2I operator -(Vector2I vec)
        {
            vec.x = -vec.x;
            vec.y = -vec.y;
            return vec;
        }

        public static Vector2I operator *(Vector2I vec, int scale)
        {
            vec.x *= scale;
            vec.y *= scale;
            return vec;
        }

        public static Vector2I operator *(int scale, Vector2I vec)
        {
            vec.x *= scale;
            vec.y *= scale;
            return vec;
        }

        public static Vector2I operator *(Vector2I left, Vector2I right)
        {
            left.x *= right.x;
            left.y *= right.y;
            return left;
        }

        public static Vector2I operator /(Vector2I vec, int divisor)
        {
            vec.x /= divisor;
            vec.y /= divisor;
            return vec;
        }

        public static Vector2I operator /(Vector2I vec, Vector2I divisorv)
        {
            vec.x /= divisorv.x;
            vec.y /= divisorv.y;
            return vec;
        }

        public static Vector2I operator %(Vector2I vec, int divisor)
        {
            vec.x %= divisor;
            vec.y %= divisor;
            return vec;
        }

        public static Vector2I operator %(Vector2I vec, Vector2I divisorv)
        {
            vec.x %= divisorv.x;
            vec.y %= divisorv.y;
            return vec;
        }

        public static Vector2I operator &(Vector2I vec, int and)
        {
            vec.x &= and;
            vec.y &= and;
            return vec;
        }

        public static Vector2I operator &(Vector2I vec, Vector2I andv)
        {
            vec.x &= andv.x;
            vec.y &= andv.y;
            return vec;
        }

        public static bool operator ==(Vector2I left, Vector2I right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2I left, Vector2I right)
        {
            return !left.Equals(right);
        }

        public static bool operator <(Vector2I left, Vector2I right)
        {
            if (left.x.Equals(right.x))
            {
                return left.y < right.y;
            }
            return left.x < right.x;
        }

        public static bool operator >(Vector2I left, Vector2I right)
        {
            if (left.x.Equals(right.x))
            {
                return left.y > right.y;
            }
            return left.x > right.x;
        }

        public static bool operator <=(Vector2I left, Vector2I right)
        {
            if (left.x.Equals(right.x))
            {
                return left.y <= right.y;
            }
            return left.x <= right.x;
        }

        public static bool operator >=(Vector2I left, Vector2I right)
        {
            if (left.x.Equals(right.x))
            {
                return left.y >= right.y;
            }
            return left.x >= right.x;
        }

#if UNITY_5_3_OR_NEWER
        public static implicit operator Vector2Int(Vector2I value)
        {
            return new Vector2Int(value.x, value.y);
        }

        public static explicit operator Vector2I(Vector2Int value)
        {
            return new Vector2I(value);
        }
#endif

        public override bool Equals(object obj)
        {
            if (obj is Vector2I)
            {
                return Equals((Vector2I)obj);
            }

            return false;
        }

        public bool Equals(Vector2I other)
        {
            return x == other.x && y == other.y;
        }

        public override int GetHashCode()
        {
            return y.GetHashCode() ^ x.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("({0}, {1})", new object[]
            {
                this.x.ToString(),
                this.y.ToString()
            });
        }

        public string ToString(string format)
        {
            return String.Format("({0}, {1})", new object[]
            {
                this.x.ToString(format),
                this.y.ToString(format)
            });
        }
    }
}
