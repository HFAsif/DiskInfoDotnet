

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace FrameWorkEngine;

//
// Summary:
//     Provides a mechanism that synchronizes access to objects.
[ComVisible(true)]
[HostProtection(SecurityAction.LinkDemand, Synchronization = true, ExternalThreading = true)]
public static class MonitorEx
{
    //
    // Summary:
    //     Acquires an exclusive lock on the specified object.
    //
    // Parameters:
    //   obj:
    //     The object on which to acquire the monitor lock.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The obj parameter is null.
    [MethodImpl(MethodImplOptions.InternalCall)]
    public static extern void Enter(object obj);

    [MethodImpl(MethodImplOptions.InternalCall)]
    internal static extern void ReliableEnter(object obj, ref bool tookLock);

    //
    // Summary:
    //     Releases an exclusive lock on the specified object.
    //
    // Parameters:
    //   obj:
    //     The object on which to release the lock.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The obj parameter is null.
    //
    //   T:System.Threading.SynchronizationLockException:
    //     The current thread does not own the lock for the specified object.
    [MethodImpl(MethodImplOptions.InternalCall)]
    [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
    public static extern void Exit(object obj);

    //
    // Summary:
    //     Attempts to acquire an exclusive lock on the specified object.
    //
    // Parameters:
    //   obj:
    //     The object on which to acquire the lock.
    //
    // Returns:
    //     true if the current thread acquires the lock; otherwise, false.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The obj parameter is null.
    public static bool TryEnter(object obj)
    {
        return TryEnterTimeout(obj, 0);
    }

    //
    // Summary:
    //     Attempts, for the specified number of milliseconds, to acquire an exclusive lock
    //     on the specified object.
    //
    // Parameters:
    //   obj:
    //     The object on which to acquire the lock.
    //
    //   millisecondsTimeout:
    //     The number of milliseconds to wait for the lock.
    //
    // Returns:
    //     true if the current thread acquires the lock; otherwise, false.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The obj parameter is null.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     millisecondsTimeout is negative, and not equal to System.Threading.Timeout.Infinite.
    public static bool TryEnter(object obj, int millisecondsTimeout)
    {
        return TryEnterTimeout(obj, millisecondsTimeout);
    }

    //
    // Summary:
    //     Attempts, for the specified amount of time, to acquire an exclusive lock on the
    //     specified object.
    //
    // Parameters:
    //   obj:
    //     The object on which to acquire the lock.
    //
    //   timeout:
    //     A System.TimeSpan representing the amount of time to wait for the lock. A value
    //     of –1 millisecond specifies an infinite wait.
    //
    // Returns:
    //     true if the current thread acquires the lock without blocking; otherwise, false.
    //
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The obj parameter is null.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     The value of timeout in milliseconds is negative and is not equal to System.Threading.Timeout.Infinite
    //     (–1 millisecond), or is greater than System.Int32.MaxValue.
    public static bool TryEnter(object obj, TimeSpan timeout)
    {
        long num = (long)timeout.TotalMilliseconds;
        if (num < -1 || num > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException("timeout", EnvironmentEx.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
        }

        return TryEnterTimeout(obj, (int)num);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool TryEnterTimeout(object obj, int timeout);

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern bool ObjWait(bool exitContext, int millisecondsTimeout, object obj);

    //
    // Summary:
    //     Releases the lock on an object and blocks the current thread until it reacquires
    //     the lock. If the specified time-out interval elapses, the thread enters the ready
    //     queue. This method also specifies whether the synchronization domain for the
    //     context (if in a synchronized context) is exited before the wait and reacquired
    //     afterward.
    //
    // Parameters:
    //   obj:
    //     The object on which to wait.
    //
    //   millisecondsTimeout:
    //     The number of milliseconds to wait before the thread enters the ready queue.
    //
    //
    //   exitContext:
    //     true to exit and reacquire the synchronization domain for the context (if in
    //     a synchronized context) before the wait; otherwise, false.
    //
    // Returns:
    //     true if the lock was reacquired before the specified time elapsed; false if the
    //     lock was reacquired after the specified time elapsed. The method does not return
    //     until the lock is reacquired.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The obj parameter is null.
    //
    //   T:System.Threading.SynchronizationLockException:
    //     Wait is not invoked from within a synchronized block of code.
    //
    //   T:System.Threading.ThreadInterruptedException:
    //     The thread that invokes Wait is later interrupted from the waiting state. This
    //     happens when another thread calls this thread's System.Threading.Thread.Interrupt
    //     method.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     The value of the millisecondsTimeout parameter is negative, and is not equal
    //     to System.Threading.Timeout.Infinite.
    public static bool Wait(object obj, int millisecondsTimeout, bool exitContext)
    {
        if (obj == null)
        {
            throw new ArgumentNullException("obj");
        }

        return ObjWait(exitContext, millisecondsTimeout, obj);
    }

    //
    // Summary:
    //     Releases the lock on an object and blocks the current thread until it reacquires
    //     the lock. If the specified time-out interval elapses, the thread enters the ready
    //     queue. Optionally exits the synchronization domain for the synchronized context
    //     before the wait and reacquires the domain afterward.
    //
    // Parameters:
    //   obj:
    //     The object on which to wait.
    //
    //   timeout:
    //     A System.TimeSpan representing the amount of time to wait before the thread enters
    //     the ready queue.
    //
    //   exitContext:
    //     true to exit and reacquire the synchronization domain for the context (if in
    //     a synchronized context) before the wait; otherwise, false.
    //
    // Returns:
    //     true if the lock was reacquired before the specified time elapsed; false if the
    //     lock was reacquired after the specified time elapsed. The method does not return
    //     until the lock is reacquired.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The obj parameter is null.
    //
    //   T:System.Threading.SynchronizationLockException:
    //     Wait is not invoked from within a synchronized block of code.
    //
    //   T:System.Threading.ThreadInterruptedException:
    //     The thread that invokes Wait is later interrupted from the waiting state. This
    //     happens when another thread calls this thread's System.Threading.Thread.Interrupt
    //     method.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     The timeout parameter is negative and does not represent System.Threading.Timeout.Infinite
    //     (–1 millisecond), or is greater than System.Int32.MaxValue.
    public static bool Wait(object obj, TimeSpan timeout, bool exitContext)
    {
        long num = (long)timeout.TotalMilliseconds;
        if (num < -1 || num > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException("timeout", EnvironmentEx.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
        }

        return Wait(obj, (int)num, exitContext);
    }

    //
    // Summary:
    //     Releases the lock on an object and blocks the current thread until it reacquires
    //     the lock. If the specified time-out interval elapses, the thread enters the ready
    //     queue.
    //
    // Parameters:
    //   obj:
    //     The object on which to wait.
    //
    //   millisecondsTimeout:
    //     The number of milliseconds to wait before the thread enters the ready queue.
    //
    //
    // Returns:
    //     true if the lock was reacquired before the specified time elapsed; false if the
    //     lock was reacquired after the specified time elapsed. The method does not return
    //     until the lock is reacquired.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The obj parameter is null.
    //
    //   T:System.Threading.SynchronizationLockException:
    //     The calling thread does not own the lock for the specified object.
    //
    //   T:System.Threading.ThreadInterruptedException:
    //     The thread that invokes Wait is later interrupted from the waiting state. This
    //     happens when another thread calls this thread's System.Threading.Thread.Interrupt
    //     method.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     The value of the millisecondsTimeout parameter is negative, and is not equal
    //     to System.Threading.Timeout.Infinite.
    public static bool Wait(object obj, int millisecondsTimeout)
    {
        return Wait(obj, millisecondsTimeout, exitContext: false);
    }

    //
    // Summary:
    //     Releases the lock on an object and blocks the current thread until it reacquires
    //     the lock. If the specified time-out interval elapses, the thread enters the ready
    //     queue.
    //
    // Parameters:
    //   obj:
    //     The object on which to wait.
    //
    //   timeout:
    //     A System.TimeSpan representing the amount of time to wait before the thread enters
    //     the ready queue.
    //
    // Returns:
    //     true if the lock was reacquired before the specified time elapsed; false if the
    //     lock was reacquired after the specified time elapsed. The method does not return
    //     until the lock is reacquired.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The obj parameter is null.
    //
    //   T:System.Threading.SynchronizationLockException:
    //     The calling thread does not own the lock for the specified object.
    //
    //   T:System.Threading.ThreadInterruptedException:
    //     The thread that invokes Wait is later interrupted from the waiting state. This
    //     happens when another thread calls this thread's System.Threading.Thread.Interrupt
    //     method.
    //
    //   T:System.ArgumentOutOfRangeException:
    //     The value of the timeout parameter in milliseconds is negative and does not represent
    //     System.Threading.Timeout.Infinite (–1 millisecond), or is greater than System.Int32.MaxValue.
    public static bool Wait(object obj, TimeSpan timeout)
    {
        long num = (long)timeout.TotalMilliseconds;
        if (num < -1 || num > int.MaxValue)
        {
            throw new ArgumentOutOfRangeException("timeout", EnvironmentEx.GetResourceString("ArgumentOutOfRange_NeedNonNegOrNegative1"));
        }

        return Wait(obj, (int)num, exitContext: false);
    }

    //
    // Summary:
    //     Releases the lock on an object and blocks the current thread until it reacquires
    //     the lock.
    //
    // Parameters:
    //   obj:
    //     The object on which to wait.
    //
    // Returns:
    //     true if the call returned because the caller reacquired the lock for the specified
    //     object. This method does not return if the lock is not reacquired.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The obj parameter is null.
    //
    //   T:System.Threading.SynchronizationLockException:
    //     The calling thread does not own the lock for the specified object.
    //
    //   T:System.Threading.ThreadInterruptedException:
    //     The thread that invokes Wait is later interrupted from the waiting state. This
    //     happens when another thread calls this thread's System.Threading.Thread.Interrupt
    //     method.
    public static bool Wait(object obj)
    {
        return Wait(obj, -1, exitContext: false);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ObjPulse(object obj);

    //
    // Summary:
    //     Notifies a thread in the waiting queue of a change in the locked object's state.
    //
    //
    // Parameters:
    //   obj:
    //     The object a thread is waiting for.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The obj parameter is null.
    //
    //   T:System.Threading.SynchronizationLockException:
    //     The calling thread does not own the lock for the specified object.
    public static void Pulse(object obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException("obj");
        }

        ObjPulse(obj);
    }

    [MethodImpl(MethodImplOptions.InternalCall)]
    private static extern void ObjPulseAll(object obj);


    [__DynamicallyInvokable]
    public static void Enter(object obj, ref bool lockTaken)
    {
        if (lockTaken)
        {
            ThrowLockTakenException();
        }

        ReliableEnter(obj, ref lockTaken);
    }

    private static void ThrowLockTakenException()
    {
        throw new ArgumentException(EnvironmentEx.GetResourceString("Argument_MustBeFalse"), "lockTaken");
    }

    //
    // Summary:
    //     Notifies all waiting threads of a change in the object's state.
    //
    // Parameters:
    //   obj:
    //     The object that sends the pulse.
    //
    // Exceptions:
    //   T:System.ArgumentNullException:
    //     The obj parameter is null.
    //
    //   T:System.Threading.SynchronizationLockException:
    //     The calling thread does not own the lock for the specified object.
    public static void PulseAll(object obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException("obj");
        }

        ObjPulseAll(obj);
    }
}
#if false // Decompilation log
'8' items in cache
#endif

