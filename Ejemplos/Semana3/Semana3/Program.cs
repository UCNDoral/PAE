using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semana3
{

    public class CuentaBancaria
    {
        public string Titular { get; set; }
        private decimal saldo;


        //Propiedad publica
        public decimal Saldo
        {
            get
            { 
                return saldo; 
            }
            private set
            {
                saldo = value;
            }
            
        }

        public CuentaBancaria(string Nombre,decimal SaldoInicial)
        {
            Titular = Nombre;

            if (SaldoInicial < 0)
                throw new ArgumentException("El saldo inicial no puede ser negativo.");
            saldo = SaldoInicial;
        }


        public void Depositar(decimal monto)
        {
            if (monto < 0)
                throw new ArgumentException("El monto debe de ser mayor que cero.");
            saldo += monto;
        }

        public bool Retirar(decimal monto)
        {
            if (monto < 0)
                throw new ArgumentException("El monto debe de ser mayor que cero.");
            if (monto > saldo)
                return false; // No se puede retirar más de lo que hay en la cuenta
            saldo -= monto;
            return true; // Retiro exitoso
        }

    }


    public class Empleado 
    {
        public string Nombre { get; set; }
        public decimal SalarioBase { get; set; }

        public Empleado( string nombre, decimal salarioBase)
        {
            
            Nombre = nombre;
            SalarioBase = salarioBase;
        }

        public virtual decimal CalcularPago()
        {
            return SalarioBase;
        }

        public virtual void MostrarDatos()
        {
            Console.WriteLine($"Empleado: {Nombre}, Salario Base: {SalarioBase}");
            
        }

    }

    public class Programador : Empleado
    {
        public decimal Bono { get; set; }

        public Programador(string nombre, decimal salarioBase, decimal bono) : base(nombre, salarioBase)
        {
            Bono = bono;
        }


        public override decimal CalcularPago()
        {
            return SalarioBase + Bono;
        }

        public override void MostrarDatos()
        {
            base.MostrarDatos();
            Console.WriteLine($"Bono: {Bono}, Pago Total: {CalcularPago()}");
        }
    }

    public class Gerente : Empleado
    {
        public decimal Comision { get; set; }

        public Gerente(string nombre, decimal salarioBase, decimal comision) : base(nombre, salarioBase)
        {
            Comision = comision;
        }

        public override decimal CalcularPago()
        {
            return SalarioBase + Comision;
        }
        public override void MostrarDatos()
        {
            base.MostrarDatos();
            Console.WriteLine($"Comisión: {Comision}, Pago Total: {CalcularPago()}");
        }
    }



    internal class Program
    {
        static void Main(string[] args)
        {

            //CuentaBancaria cuenta = new CuentaBancaria("Angel Lars", 1000);

            //Console.WriteLine($"La cuenta de {cuenta.Titular} tiene un saldo de {cuenta.Saldo}");

            //Console.WriteLine("Deposito");

            //cuenta.Depositar(500);

            //Console.WriteLine($"La cuenta de {cuenta.Titular} tiene un saldo de {cuenta.Saldo}");

            //Console.WriteLine("Retiro");

            //bool respuesta = cuenta.Retirar(200);

            //Console.WriteLine($"La cuenta de {cuenta.Titular} tiene un saldo de {cuenta.Saldo}");


            Programador Angel = new Programador("Angel Lars", 50000, 10000);

            Gerente Maria = new Gerente("Maria Gomez", 70000, 15000);


            Console.WriteLine($"Datos del Programador: {Angel.Nombre} tiene un salarios base de {Angel.SalarioBase} y este recibe un bono de {Angel.Bono}");
            Console.WriteLine($"El pago total del Programador es: {Angel.CalcularPago()}");

            Console.WriteLine($"Datos del Gerente: {Maria.Nombre} tiene un salarios base de {Maria.SalarioBase} y este recibe una comisión de {Maria.Comision}");
            Console.WriteLine($"El pago total del Gerente es: {Maria.CalcularPago()}");






        }
    }
}
