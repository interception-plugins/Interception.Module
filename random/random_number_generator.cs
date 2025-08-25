using System;

namespace interception.random {
    public abstract class random_number_generator {
        protected const double FACT = 2.32830643653869628906e-10;

        protected abstract double generate();
        
        public virtual double value => generate();
        
        public virtual double rand_double(double inclusive_min, double exclusive_max) {
            if (inclusive_min > exclusive_max)
                throw new ArgumentException("min value cannot be lower than max value");

            return (this.generate() * (exclusive_max - inclusive_min)) + inclusive_min;
        }

        public virtual double rand_double(double exclusive_max) {
            return this.generate() * exclusive_max;
        }

        public virtual int rand_int() {
            return (int)Math.Floor(rand_double(-int.MaxValue * 1.0, int.MaxValue * 1.0));
        }

        public virtual int rand_int(int inclusive_min, int exclusive_max) {
            if (inclusive_min > exclusive_max)
                throw new ArgumentException("min value cannot be lower than max value");
            return (int)Math.Floor(rand_double(inclusive_min * 1.0, exclusive_max * 1.0));
        }

        public virtual int rand_int(int exclusive_max) {
            return (int)Math.Floor(rand_double(0.0, exclusive_max * 1.0));
        }

        public virtual void rand_bytes(byte[] buffer) {
            if (buffer == null)
                throw new NullReferenceException("buffer is null");
            var len = buffer.Length;
            for (int i = 0; i < len; i++)
                buffer[i] = (byte)Math.Floor(rand_double(byte.MinValue, byte.MaxValue + 1));
        }

        public virtual void rand_bytes(byte[] buffer, int start_index, int length) {
            if (buffer == null)
                throw new NullReferenceException("buffer is null");
            if (start_index < 0)
                throw new ArgumentOutOfRangeException("start_index cannot be lower than zero");
            if (length < 0)
                throw new ArgumentOutOfRangeException("length cannot be lower than zero");
            if (buffer.Length - start_index < length)
                throw new ArgumentOutOfRangeException("invalid length");
            for (int i = start_index; i < start_index + length; i++)
                buffer[i] = (byte)Math.Floor(rand_double(byte.MinValue, byte.MaxValue + 1));
        }
    }
}
