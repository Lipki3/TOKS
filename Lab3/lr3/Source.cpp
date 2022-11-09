#include <iostream>
#include <vector>

using namespace std;

int main()
{
    setlocale(LC_ALL, "Russian");
    char str1[10];
    char str2[14];
    int st1[10];
    int code1[14];

    int dec1[14];
    int dec2[14];
    int k1 = 0, k2 = 0, k3 = 0, k4 = 0; int v = 0;
    int l1 = 0, l2 = 0, l3 = 0, l4 = 0; int b = 0;
    int c1 = 0, c2 = 0, c3 = 0, c4 = 0; int m = 0;
    cout << "Введите первое число: ";
    for (int i = 0; i < 10; i++) {
        cin >> str1[i];
    }

    for (int i = 0; i < 10; i++) {
        st1[i] = int(str1[i]) - 48;

    }

    for (int i = 0, j = 0; i < 14; i++) {
        if (i == 0 || i == 1 || i == 3 || i == 7) {
            code1[i] = 0;

        }
        else {
            code1[i] = st1[j];

            j++;
        }
    }
    cout << "\nПервый код: ";
    for (int i = 0; i < 14; i++) {
        cout << code1[i];
    }


    for (int i = 0; i < 14; i++) {
        dec1[i] = code1[i];
    }



    int count = 0; int count2 = 0;
    for (int i = 0; i < 14; i++) {
        if (i == 0 || i == 2 || i == 4 || i == 6 || i == 8 || i == 10 || i == 12) {
            count = count + code1[i];
        }

    }
    count = count % 2;
    if (count == 1) dec1[0] = 1;

    count = 0; count2 = 0;
    for (int i = 0; i < 14; i++) {
        if (i == 1 || i == 2 || i == 5 || i == 6 || i == 9 || i == 10) {
            count = count + code1[i];
        }
    }
    count = count % 2;
    if (count == 1) dec1[1] = 1;

    count = 0; count2 = 0;
    for (int i = 0; i < 14; i++) {
        if (i == 3 || i == 4 || i == 5 || i == 6 || i == 11 || i == 12) {
            count = count + code1[i];
        }
    }
    count = count % 2;
    if (count == 1) dec1[3] = 1;

    count = 0; count2 = 0;
    for (int i = 0; i < 14; i++) {
        if (i == 7 || i == 8 || i == 9 || i == 10 || i == 11 || i == 12 || i == 13) {
            count = count + code1[i];
        }
    }
    count = count % 2;
    if (count == 1) dec1[7] = 1;

    cout << "\nФинальная форма первого числа: ";
    for (int i = 0; i < 14; i++) {
        cout << dec1[i];
    }

    cout << "\nВведите второе число:          ";
    for (int i = 0; i < 14; i++) {
        cin >> str2[i];
    }
    for (int i = 0; i < 14; i++) {
        dec2[i] = int(str2[i]) - 48;
    }


    cout << "\nФинальная форма второго числа: ";
    for (int i = 0; i < 14; i++) {
        cout << dec2[i];
    }

    v = 0; b = 0;
    for (int i = 0; i < 14; i++) {
        v = v + dec1[i];
        b = b + dec2[i];
    }
    v = v % 2;
    b = b % 2;
    if (v != b) m = 1;
    // cout << "\nПервое V: " << v << "  Второе V: " << b;


    k1 = dec1[0] + dec1[2] + dec1[4] + dec1[6] + dec1[8] + dec1[10] + dec1[12];
    l1 = dec2[0] + dec2[2] + dec2[4] + dec2[6] + dec2[8] + dec2[10] + dec2[12];
    k1 = k1 % 2;
    l1 = l1 % 2;
    if (k1 != l1) c1 = 1;
    //  cout << "\nПервое K1: " << k1 << "  Второе K1: " << l1;

    k2 = dec1[1] + dec1[2] + dec1[5] + dec1[6] + dec1[9] + dec1[10] + dec1[13];
    l2 = dec2[1] + dec2[2] + dec2[5] + dec2[6] + dec2[9] + dec2[10] + dec2[13];
    k2 = k2 % 2;
    l2 = l2 % 2;
    if (k2 != l2) c2 = 1;
    //  cout << "\nПервое K2: " << k2 << "  Второе K2: " << l2;

    k3 = dec1[3] + dec1[4] + dec1[5] + dec1[6] + dec1[11] + dec1[12] + dec1[13];
    l3 = dec2[3] + dec2[4] + dec2[5] + dec2[6] + dec2[11] + dec2[12] + dec2[13];
    k3 = k3 % 2;
    l3 = l3 % 2;
    if (k3 != l3) c3 = 1;
    //   cout << "\nПервое K3: " << k3 << "  Второе K3: " << l3;

    k4 = dec1[7] + dec1[8] + dec1[9] + dec1[10] + dec1[11] + dec1[12] + dec1[13];
    l4 = dec2[7] + dec2[8] + dec2[9] + dec2[10] + dec2[11] + dec2[12] + dec2[13];
    k4 = k4 % 2;
    l4 = l4 % 2;
    if (k4 != l4) c4 = 1;
    //  cout << "\nПервое K4: " << k3 << "  Второе K4: " << l3;


    cout << "\n\nC = 0, V = 0 -- 0 ошибок\nC = 0, V = 1 -- 1 ошибка (в восьмом разряде)\nC != 0, V = 0 -- 2 ошибки\nС != 0, V = 1 -- 1 ошибка";
    cout << "\nC: " << c4 << c3 << c2 << c1 << "  V = " << m;


    if ((c1 + c2 + c3 + c4 == 0) && m == 0) cout << "\nОшибок нет";
    int num = 0;
    if ((c1 == 1 || c2 == 1 || c3 == 1 || c4 == 1) && m == 1) {
        if (c1 == 1) num = num + 1;
        if (c2 == 1) num = num + 2;
        if (c3 == 1) num = num + 4;
        if (c4 == 1) num = num + 8;
        cout << "\nОшибка в разряде " << num;

    }
    if ((c1 + c2 + c3 + c4 == 0) && m == 1) cout << "\nОшибка в разряде 8";
    if ((c1 == 1 || c2 == 1 || c3 == 1 || c4 == 1) && m == 0) cout << "\nДве ошибки";

    cout << "\n";
    cout << "\n";
    return 0;
}
