using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// Класс длинной арифметики
/// </summary>
public class LongArithmetic : IEnumerable
{
    /// <summary>
    /// Основание системы исчисления
    /// </summary>
    private const int _base = 10000;

    /// <summary>
    /// Максимальная длина чисел, которые могут храниться в ячейке
    /// </summary>
    private const int _baselen = 4;

    /// <summary>
    /// Указание знака числа
    /// </summary>
    private bool _isPositive;

    /// <summary>
    /// Хранящееся число в виде списка
    /// </summary>
    private List<ushort> _number;

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public LongArithmetic()
    {
        _number = new List<ushort>();
        _number.Add(0);
        _isPositive = true;
    }

    /// <summary>
    /// Конструктор с параметром
    /// </summary>
    /// <param name="num">Число длинной арифметики</param>
    public LongArithmetic(LongArithmetic num)
    {
        _isPositive = num._isPositive;
        _number = new List<ushort>(num._number);
    }

    /// <summary>
    /// Конструктор с параметром
    /// </summary>
    /// <param name="num">Число длинной арифметики</param>
    public LongArithmetic(IEnumerable<ushort> num)
    {
        _isPositive = true;
        _number = new List<ushort>(num);
    }

    /// <summary>
    /// Сложение
    /// </summary>
    /// <param name="left">Левое слагаемое</param>
    /// <param name="right">Правое слагаемое</param>
    /// <returns>Сумма</returns>
    public static LongArithmetic operator +(LongArithmetic left, LongArithmetic right)
    {
        LongArithmetic res = new LongArithmetic();

        if (left._isPositive == right._isPositive)
        {
            res._isPositive = left._isPositive;
            res._number = new List<ushort>();

            int i;
            ushort t = 0;
            for (i = 0; i < left._number.Count && i < right._number.Count; i++)
            {
                t = (ushort)(left._number[i] + right._number[i] + t);

                res._number.Add((ushort)(t % _base));
                t /= _base;
            }

            for (; i < left._number.Count; i++)
            {
                t = (ushort)(left._number[i] + t);
                res._number.Add((ushort)(t % _base));
                t /= _base;
            }

            for (; i < right._number.Count; i++)
            {
                t = (ushort)(right._number[i] + t);
                res._number.Add((ushort)(t % _base));
                t /= _base;
            }

            if (t != 0)
            {
                res._number.Add(t);
            }
        }
        else if (!left._isPositive)
        {
            left._isPositive = true;
            res = right - left;
            left._isPositive = false;
        }
        else
        {
            right._isPositive = true;
            res = left - right;
            right._isPositive = false;
        }

        return res;
    }

    /// <summary>
    /// Вычитание
    /// </summary>
    /// <param name="left">Уменьшаемое</param>
    /// <param name="right">Вычитаемое</param>
    /// <returns>Разность</returns>
    public static LongArithmetic operator -(LongArithmetic left, LongArithmetic right)
    {
        LongArithmetic res = new LongArithmetic();
        if (left._isPositive && right._isPositive)
        {
            if (left < right)
            {
                res = right - left;
                res._isPositive = false;
            }
            else
            {
                res._isPositive = true;

                res._number = new List<ushort>();

                int i;
                int t = 0;

                for (i = 0; i < left._number.Count && i < right._number.Count; i++)
                {
                    t = (t + left._number[i] - right._number[i]);
                    res._number.Add((ushort)((t + _base) % _base));
                    t = (t + _base) / _base - 1;
                }

                for (; i < left._number.Count; i++)
                {
                    t = (t + left._number[i]);
                    res._number.Add((ushort)((t + _base) % _base));
                    t = (t + _base) / _base - 1;
                }
                for (i = res._number.Count - 1; i >= 0 && res._number[i] == 0; i--)
                {
                    res._number.RemoveAt(i);
                }

                if (res._number.Count == 0)
                {
                    res._number.Add(0);
                }
            }
        }
        else if (!left._isPositive && !right._isPositive)
        {
            left._isPositive = right._isPositive = true;
            res = right - left;
            left._isPositive = right._isPositive = false;
        }
        else if (!left._isPositive)
        {
            right._isPositive = false;
            res = left + right;
            right._isPositive = true;
        }
        else
        {
            right._isPositive = true;
            res = left + right;
            right._isPositive = false;
        }

        return res;
    }

    /// <summary>
    /// Умножение
    /// </summary>
    /// <param name="left">Левый множитель</param>
    /// <param name="right">Правый множитель</param>
    /// <returns>Произведение</returns>
    public static LongArithmetic operator *(LongArithmetic left, LongArithmetic right)
    {
        LongArithmetic res = new LongArithmetic();
        res._number = new List<ushort>(left._number.Count + right._number.Count);

        if (left._isPositive == right._isPositive)
        {
            res._isPositive = true;
        }
        else
        {
            res._isPositive = false;
        }

        int t;
        int i;

        for (i = 0; i < left._number.Count; i++)
        {
            int j;
            t = 0;
            for (j = 0; j < right._number.Count; j++)
            {
                if (i + j >= res._number.Count)
                {
                    res._number.Add(0);
                }

                t = left._number[i] * right._number[j] + t + res._number[i + j];
                res._number[i + j] = ((ushort)(t % _base));
                t /= _base;
            }
            for (int k = 0; t != 0; k++)
            {
                if (i + j + k >= res._number.Count)
                {
                    res._number.Add(0);
                }
                t += res._number[i + j + k];
                res._number[i + j + k] = ((ushort)(t % _base));
                t /= _base;
            }
        }

        for (i = res._number.Count - 1; i >= 0 && res._number[i] == 0; i--)
        {
            res._number.RemoveAt(i);
        }

        if (res._number.Count == 0)
        {
            res._number.Add(0);
        }

        return res;
    }

    /// <summary>
    /// Деление
    /// </summary>
    /// <param name="left">Делимое</param>
    /// <param name="right">Делитель</param>
    /// <returns>Частое</returns>
    public static LongArithmetic operator /(LongArithmetic left, LongArithmetic right)
    {
        if (right == 0)
        {
            throw new DivideByZeroException();
        }

        bool leftpos = left._isPositive;
        bool rightpos = right._isPositive;

        if(!leftpos)
        {
            left._isPositive = true;
        }

        if (!rightpos)
        {
            right._isPositive = true;
        }

        LongArithmetic l = 0;
        LongArithmetic r = left + 1;
        LongArithmetic m;

        while (l + 1 != r)
        {
            m = DivS(l + r, 2);
            LongArithmetic mr = m * right;
            if (mr > left)
            {
                r = m;
            }
            else if (mr < left)
            {
                l = m;
            }
            else
            {
                return m;
            }
        }

        if (leftpos == rightpos)
        {
            l._isPositive = true;
        }
        else
        {
            l._isPositive = false;
        }

        left._isPositive = leftpos;
        right._isPositive = rightpos;

        return l;
    }

    /// <summary>
    /// Остаток
    /// </summary>
    /// <param name="left">Делимое</param>
    /// <param name="right">Делитель</param>
    /// <returns>Остаток</returns>
    public static LongArithmetic operator %(LongArithmetic left, LongArithmetic right)
    {
        LongArithmetic res = left - ((left / right) * right);
        if(res < 0)
        {
            bool rightsign = right._isPositive;
            right._isPositive = true;
            res += right;
            right._isPositive = rightsign;
        }
        return res;
    }

    /// <summary>
    /// Деление для коротких чисел
    /// </summary>
    /// <param name="left">Делимое</param>
    /// <param name="right">Делитель</param>
    /// <returns>Частное</returns>
    public static LongArithmetic DivS(LongArithmetic left, int right)
    {
        LongArithmetic res = new LongArithmetic();
        res._isPositive = left._isPositive;
        res._number = new List<ushort>();

        if (right < 0)
        {
            res._isPositive = !res._isPositive;
        }

        int t = 0, r;

        for (int i = left._number.Count - 1; i >= 0; i--)
        {
            r = (left._number[i] + t) / right;
            res._number.Add((ushort)r);
            t = left._number[i] + t - r * right;
            t *= _base;
        }

        res._number.Reverse();

        for (int i = res._number.Count - 1; i >= 0 && res._number[i] == 0; i--)
        {
            res._number.RemoveAt(i);
        }

        if (res._number.Count == 0)
        {
            res._number.Add(0);
        }

        return res;
    }

    public static LongArithmetic ModS(LongArithmetic left, int right)
    {
        LongArithmetic res = left - ((DivS(left, right)) * right);
        if (res < 0)
        {
            bool rightsign = right >= 0;
            if(!rightsign)
            {
                right *= -1;
            }
            res += right;
        }
        return res;
    }

    public bool IsZero()
    {
        return _number.Count == 1 && _number[0] == 0;
    }

    public bool Mod2()
    {
        return _number[0] % 2 == 1;
    }

    /// <summary>
    /// Сравнение больше
    /// </summary>
    /// <param name="left">Левый параметр</param>
    /// <param name="right">Правый параметр</param>
    /// <returns>true, если значение параметра left больше значения параметра right. в противном случае — false.</returns>
    public static bool operator >(LongArithmetic left, LongArithmetic right)
    {
        if (left._isPositive && !right._isPositive)
        {
            return true;
        }
        else if (!left._isPositive && right._isPositive)
        {
            return false;
        }
        else if (left._isPositive && right._isPositive)
        {
            if (left._number.Count > right._number.Count)
            {
                return true;
            }
            else if (left._number.Count < right._number.Count)
            {
                return false;
            }
            for (int i = left._number.Count - 1; i >= 0; i--)
            {
                if (left._number[i] > right._number[i])
                {
                    return true;
                }
                else if (left._number[i] < right._number[i])
                {
                    return false;
                }
            }
            return false;
        }
        else
        {
            if (left._number.Count < right._number.Count)
            {
                return true;
            }
            else if (left._number.Count > right._number.Count)
            {
                return false;
            }
            for (int i = left._number.Count - 1; i >= 0; i--)
            {
                if (left._number[i] < right._number[i])
                {
                    return true;
                }
                else if (left._number[i] > right._number[i])
                {
                    return false;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Сравнение меньше
    /// </summary>
    /// <param name="left">Левый параметр</param>
    /// <param name="right">Правый параметр</param>
    /// <returns>true, если значение параметра left меньше значения параметра right. в противном случае — false.</returns>
    public static bool operator <(LongArithmetic left, LongArithmetic right)
    {
        if (left._isPositive && !right._isPositive)
        {
            return false;
        }
        else if (!left._isPositive && right._isPositive)
        {
            return true;
        }
        else if (left._isPositive && right._isPositive)
        {
            if (left._number.Count > right._number.Count)
            {
                return false;
            }
            else if (left._number.Count < right._number.Count)
            {
                return true;
            }
            for (int i = left._number.Count - 1; i >= 0; i--)
            {
                if (left._number[i] > right._number[i])
                {
                    return false;
                }
                else if (left._number[i] < right._number[i])
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            if (left._number.Count < right._number.Count)
            {
                return false;
            }
            else if (left._number.Count > right._number.Count)
            {
                return true;
            }
            for (int i = left._number.Count - 1; i >= 0; i--)
            {
                if (left._number[i] < right._number[i])
                {
                    return false;
                }
                else if (left._number[i] > right._number[i])
                {
                    return true;
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Сравнение на равность
    /// </summary>
    /// <param name="left">Левый параметр</param>
    /// <param name="right">Правый параметр</param>
    /// <returns>true, если значение параметра left равно значению параметра right. в противном случае — false.</returns>
    public static bool operator ==(LongArithmetic left, LongArithmetic right)
    {
        if (((left._isPositive && right._isPositive) || (!left._isPositive && !right._isPositive)) && left._number.Count == right._number.Count)
        {
            for (int i = left._number.Count - 1; i >= 0; i--)
            {
                if (left._number[i] != right._number[i])
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Сравнение на не равность
    /// </summary>
    /// <param name="left">Левый параметр</param>
    /// <param name="right">Правый параметр</param>
    /// <returns>true, если значение параметра left не равно значению параметра right. в противном случае — false.</returns>
    public static bool operator !=(LongArithmetic left, LongArithmetic right)
    {
        return !(left == right);
    }

    /// <summary>
    /// Сравнение на больше либо равность
    /// </summary>
    /// <param name="left">Левый параметр</param>
    /// <param name="right">Правый параметр</param>
    /// <returns>true, если значение параметра left больше либо равно значению параметра right. в противном случае — false.</returns>
    public static bool operator >=(LongArithmetic left, LongArithmetic right)
    {
        return left > right || left == right;
    }

    /// <summary>
    /// Сравнение на меньше либо равность
    /// </summary>
    /// <param name="left">Левый параметр</param>
    /// <param name="right">Правый параметр</param>
    /// <returns>true, если значение параметра left меньше либо равно значению параметра right. в противном случае — false.</returns>
    public static bool operator <=(LongArithmetic left, LongArithmetic right)
    {
        return left < right || left == right;
    }

    /// <summary>
    /// Явное преобразование для bool
    /// </summary>
    /// <param name="num">Параметр для преобразования</param>
    public static implicit operator LongArithmetic(bool num)
    {
        if (num)
            return 1;
        else
            return 0;
    }

    /// <summary>
    /// Явное преобразование для sbyte
    /// </summary>
    /// <param name="num">Параметр для преобразования</param>
    public static implicit operator LongArithmetic(sbyte num)
    {
        return (long)num;
    }

    /// <summary>
    /// Явное преобразование для byte
    /// </summary>
    /// <param name="num">Параметр для преобразования</param>
    public static implicit operator LongArithmetic(byte num)
    {
        return (ulong)num;
    }

    /// <summary>
    /// Явное преобразование для short
    /// </summary>
    /// <param name="num">Параметр для преобразования</param>
    public static implicit operator LongArithmetic(short num)
    {
        return (long)num;
    }

    /// <summary>
    /// Явное преобразование для ushort
    /// </summary>
    /// <param name="num">Параметр для преобразования</param>
    public static implicit operator LongArithmetic(ushort num)
    {
        return (ulong)num;
    }

    /// <summary>
    /// Явное преобразование для int
    /// </summary>
    /// <param name="num">Параметр для преобразования</param>
    public static implicit operator LongArithmetic(int num)
    {
        return (long)num;
    }

    /// <summary>
    /// Явное преобразование для uint
    /// </summary>
    /// <param name="num">Параметр для преобразования</param>
    public static implicit operator LongArithmetic(uint num)
    {
        return (ulong)num;
    }

    /// <summary>
    /// Явное преобразование для long
    /// </summary>
    /// <param name="num">Параметр для преобразования</param>
    public static implicit operator LongArithmetic(long num)
    {
        bool pos = true;
        if (num < 0)
        {
            pos = false;
            num *= -1;
        }

        LongArithmetic res = (ulong)num;
        res._isPositive = pos;

        return res;
    }

    /// <summary>
    /// Явное преобразование для ulong
    /// </summary>
    /// <param name="num">Параметр для преобразования</param>
    public static implicit operator LongArithmetic(ulong num)
    {
        string snum = num.ToString();

        return Parse(snum);
    }

    /// <summary>
    /// Преобразует строковое представление числа в эквивалентное ему длинное целое число со знаком.
    /// </summary>
    /// <param name="s">Строка, содержащая преобразуемое число.</param>
    /// <returns>Длинное целое число со знаком, эквивалентное числу, заданному в параметре s.</returns>
    public static LongArithmetic Parse(string s)
    {
        LongArithmetic res = new LongArithmetic();
        res._number = new List<ushort>();

        for (int i = s.Length - 1; i >= 0;)
        {
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < _baselen && i >= 0; j++, i--)
            {
                if (i == 0 && s[i] == '-')
                {
                    res._isPositive = false;
                    continue;
                }
                sb.Append(s[i]);
            }

            string news = new string(sb.ToString().ToCharArray().Reverse().ToArray());

            if (news != "")
            {
                res._number.Add(ushort.Parse(news));
            }
        }
        return res;
    }

    /// <summary>
    /// Преобразует числовое значение данного экземпляра в эквивалентное строковое представление с использованием указанного формата.
    /// </summary>
    /// <returns>Строковое представление значения данного экземпляра, состоящее из знака минус,если число отрицательное, и последовательности цифр в диапазоне от 0 до 9 с ненулевой первой цифрой.</returns>
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        if (!_isPositive)
        {
            sb.Append('-');
        }

        for (int i = _number.Count - 1; i >= 0; i--)
        {
            if (i != _number.Count - 1)
            {
                string ts = _number[i].ToString();
                for (int k = 0; k < _baselen - ts.Length; k++)
                {
                    sb.Append('0');
                }

            }
            sb.Append(_number[i]);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Возвращает хэш-код данного экземпляра.
    /// </summary>
    /// <returns>Хэш-код в виде 32-разрядного целого числа со знаком.</returns>
    public override int GetHashCode()
    {
        return _number.GetHashCode() * (_isPositive ? 1 : -1);
    }

    /// <summary>
    /// Возвращает значение, указывающее, равен ли этот экземпляр заданному значению типа LongArithmetic
    /// </summary>
    /// <param name="obj">Значение типа LongArithmetic для сравнения с данным экземпляром.</param>
    /// <returns>true, если значение параметра obj совпадает со значением данного экземпляра. в противном случае — false.</returns>
    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            LongArithmetic ml = (LongArithmetic)obj;
            return this == ml;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _number.GetEnumerator();
    }
}
