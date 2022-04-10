using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KalkuzSystems.DataStructures.AstronomicalNumbers
{
    [Serializable]
    public struct AstronomicalNumber
    {
        private const uint MAX_OPERATION_DEGREE_DIFFERENCE = 2;

        [Min(0)] private float value;
        [SerializeField] private AstronomicalIdentifier degree;

        public float Value => value;
        public float Degree => (int)degree;

        #region To Strings

        public string NumericString => value.ToString("0.000");
        public string DegreeShort => degree.ToString();
        public string DegreeLong => degree.ToString();
        
        public string ShortName()
        {
            if (degree == AstronomicalIdentifier.Max) return "Max";
            else if (degree == AstronomicalIdentifier.DEFAULT) return value.ToString("0.00");
            else return $"{NumericString}{DegreeShort}";
        }
        
        public string LongName()
        {
            if (degree == AstronomicalIdentifier.Max) return "Max";
            else if (degree == AstronomicalIdentifier.DEFAULT) return value.ToString("0.00");
            else return $"{NumericString} {DegreeLong}";
        }

        public override string ToString()
        {
            return LongName();
        }

        #endregion
        
        public AstronomicalNumber(float value)
        {
            this.value = Mathf.Max(0, value);
            this.degree = AstronomicalIdentifier.DEFAULT;
        }
        public AstronomicalNumber(AstronomicalNumber other)
        {
            this.value = Mathf.Max(0, other.value);
            this.degree = other.degree;
        }
        public void Astronomize()
        {
            while (value >= 1000f && degree != AstronomicalIdentifier.Max)
            {
                value /= 1000f;
                degree++;
            }

            while (value < 1f && degree > 0)
            {
                value *= 1000f;
                degree--;
            }
        }

        #region Operators

        public static AstronomicalNumber operator +(AstronomicalNumber lhs, AstronomicalNumber rhs)
        {
            if (lhs.degree == AstronomicalIdentifier.Max) return lhs;
            else if (rhs.degree == AstronomicalIdentifier.Max) return rhs;

            if (lhs.degree < rhs.degree) return rhs + lhs;

            if (lhs.degree - rhs.degree > MAX_OPERATION_DEGREE_DIFFERENCE) return lhs;

            lhs.value += Mathf.Abs(rhs.value) / Mathf.Pow(1000, lhs.degree - rhs.degree);
            if (lhs.value >= 1000f)
            {
                lhs.value /= 1000f;
                lhs.degree++;
            }

            return lhs;
        }
        public static AstronomicalNumber operator +(AstronomicalNumber lhs, float rhs)
        {
            return lhs + new AstronomicalNumber(rhs);
        }
        public static AstronomicalNumber operator -(AstronomicalNumber lhs, AstronomicalNumber rhs)
        {
            lhs.value = Mathf.Max(0, lhs.value - Mathf.Abs(rhs.value) / Mathf.Pow(1000, lhs.degree - rhs.degree));

            if (lhs.value == 0f) return new AstronomicalNumber(0);

            while (lhs.value < 1f && lhs.degree != AstronomicalIdentifier.DEFAULT)
            {
                lhs.value *= 1000f;
                lhs.degree = (AstronomicalIdentifier)Mathf.Max((int)lhs.degree - 1, 0);
            }

            return lhs;
        }
        public static AstronomicalNumber operator -(AstronomicalNumber lhs, float rhs)
        {
            return lhs - new AstronomicalNumber(rhs);
        }
        public static AstronomicalNumber operator *(AstronomicalNumber lhs, AstronomicalNumber rhs)
        {
            lhs.degree = (AstronomicalIdentifier)Mathf.Clamp((int)lhs.degree + (int)rhs.degree, 0, (int)AstronomicalIdentifier.Max);
            if (lhs.degree == AstronomicalIdentifier.Max && rhs.value > 1f) return lhs;

            lhs.value *= rhs.value;

            lhs.Astronomize();

            return lhs;
        }
        public static AstronomicalNumber operator *(AstronomicalNumber lhs, float rhs)
        {
            return lhs * new AstronomicalNumber(rhs);
        }
        public static AstronomicalNumber operator /(AstronomicalNumber lhs, AstronomicalNumber rhs)
        {
            if (rhs.value == 0f) throw new DivideByZeroException();
            if (rhs.value < 1f && lhs.degree == AstronomicalIdentifier.Max) return lhs;

            int degreeDiff = (int)lhs.degree - (int)rhs.degree;
            lhs.degree = (AstronomicalIdentifier)Mathf.Clamp(degreeDiff, 0, (int)AstronomicalIdentifier.Max);
            lhs.value /= rhs.value;

            if (degreeDiff < 0)
            {
                lhs.value /= Mathf.Pow(1000, degreeDiff);
            }

            lhs.Astronomize();

            return lhs;
        }
        public static AstronomicalNumber operator /(AstronomicalNumber lhs, float rhs)
        {
            return lhs / new AstronomicalNumber(rhs);
        }

        #endregion
    }
}
