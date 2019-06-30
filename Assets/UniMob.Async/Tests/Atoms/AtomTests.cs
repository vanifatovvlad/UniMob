using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace UniMob.Async.Tests.Atoms
{
    public class AtomTests
    {
        [Test]
        public void MergeValues() => TestZone.Run(tick =>
        {
            var value = new int[0];

            var source = Atom.Value(1);
            var atom = Atom.Computed(
                pull: () => value = new[] {Math.Abs(source.Value)},
                merge: (prev, next) => next.SequenceEqual(prev) ? prev : next);

            atom.Get();
            Assert.IsTrue(ReferenceEquals(value, atom.Value));

            source.Value = -1;
            Assert.IsTrue(ReferenceEquals(value, atom.Value));
        });

        [Test]
        public void ManualActivation() => TestZone.Run(tick =>
        {
            var activation = "";

            var atom = Atom.Computed(
                pull: () => 1,
                onActive: () => activation += "A",
                onInactive: () => activation += "D");

            Assert.AreEqual("", activation);

            atom.Get();
            Assert.AreEqual("A", activation);

            atom.Deactivate();
            Assert.AreEqual("AD", activation);
        });

        [Test]
        public void AutoActivation() => TestZone.Run(tick =>
        {
            var activation = "";

            var atom = Atom.Computed(
                pull: () => 1,
                onActive: () => activation += "A",
                onInactive: () => activation += "D");
            var listener = Atom.Computed(() => atom.Value + 1);

            Assert.AreEqual("", activation);

            listener.Get();
            Assert.AreEqual("A", activation);

            listener.Deactivate();
            Assert.AreEqual("A", activation);

            tick(0);
            Assert.AreEqual("AD", activation);
        });

        [Test]
        public void NoReactivationDuringPulling() => TestZone.Run(tick =>
        {
            var activation = "";

            var activationSource = Atom.Computed(
                pull: () => 1,
                onActive: () => activation += "A",
                onInactive: () => activation += "D");

            var modifiedSource = Atom.Value(1);
            var listener = Atom.Computed(() => activationSource.Value + modifiedSource.Value);

            Assert.AreEqual("", activation);

            listener.Get();
            Assert.AreEqual("A", activation);

            modifiedSource.Value = 2;
            Assert.AreEqual("A", activation);

            tick(0);
            Assert.AreEqual("A", activation);
        });

        [Test]
        public void NoReactivationDuringModification() => TestZone.Run(tick =>
        {
            var activation = "";

            var atom = Atom.Value(1,
                onActive: () => activation += "A",
                onInactive: () => activation += "D");

            Assert.AreEqual("", activation);

            atom.Get();
            Assert.AreEqual("A", activation);

            atom.Value = 2;
            Assert.AreEqual("A", activation);

            tick(0);
            Assert.AreEqual("A", activation);
        });

        [Test]
        public void Caching() => TestZone.Run(tick =>
        {
            var random = new Random();
            var atom = Atom.Computed(() => random.Next());

            Assert.AreEqual(atom.Value, atom.Value);
        });

        [Test]
        public void Lazy() => TestZone.Run(tick =>
        {
            var value = 0;
            var atom = Atom.Computed(() => value = 1);

            tick(0f);
            Assert.AreEqual(0, value);

            atom.Get();
            Assert.AreEqual(1, value);
        });

        [Test]
        public void InstantActualization() => TestZone.Run(tick =>
        {
            var source = Atom.Value(1);
            var middle = Atom.Computed(() => source.Value + 1);
            var target = Atom.Computed(() => middle.Value + 1);

            Assert.AreEqual(3, target.Value);

            source.Value = 2;

            Assert.AreEqual(4, target.Value);
        });

        [Test]
        public void DoNotActualizeWhenMastersNotChanged() => TestZone.Run(tick =>
        {
            var targetUpdates = 0;

            var source = Atom.Value(1);
            var middle = Atom.Computed(() => Math.Abs(source.Value));
            var target = Atom.Computed(() =>
            {
                ++targetUpdates;
                return middle.Value;
            });

            target.Get();
            Assert.AreEqual(1, target.Value);

            source.Set(-1);
            target.Get();

            Assert.AreEqual(1, targetUpdates);
        });

        [Test]
        public void ObsoleteAtomsActualizedInInitialOrder() => TestZone.Run(tick =>
        {
            var actualization = "";

            var source = Atom.Value(1);
            var middle = Atom.Computed(() =>
            {
                actualization += "M";
                return source.Value;
            });
            var target = Atom.Computed(() =>
            {
                actualization += "T";
                source.Get();
                return middle.Value;
            });

            target.Get();
            Assert.AreEqual("TM", actualization);

            source.Value = 2;

            tick(0);
            Assert.AreEqual("TMTM", actualization);
        });

        [Test]
        public void AtomicDeferredRestart() => TestZone.Run(tick =>
        {
            int targetValue = 0;

            var source = Atom.Value(1);
            var middle = Atom.Computed(() => source.Value + 1);
            var target = Atom.Computed(() => targetValue = middle.Value + 1);

            target.Get();
            Assert.AreEqual(3, targetValue);

            source.Value = 2;
            Assert.AreEqual(3, targetValue);

            tick(0);
            Assert.AreEqual(4, targetValue);
        });

        [Test]
        public void SettingEqualStateAreIgnored() => TestZone.Run(tick =>
        {
            var atom = Atom.Value(new[] {1, 2, 3},
                comparer: new TestComparer<int[]>((a, b) => a.SequenceEqual(b)));

            var v1 = atom.Value;
            var v2 = new[] {1, 2, 3};
            atom.Value = v2;
            var v3 = atom.Value;

            Assert.IsTrue(ReferenceEquals(v1, v3));
            Assert.IsFalse(ReferenceEquals(v2, v3));
        });

        [Test]
        public void ThrowException() => TestZone.Run(tick =>
        {
            var source = Atom.Value(0);
            var exception = new Exception();

            var middle = Atom.Computed(() =>
            {
                if (source.Value == 0)
                    throw exception;

                return source.Value + 1;
            });

            var stack = new Stack<Exception>();

            var reaction = Atom.CreateReaction(
                reaction: () => middle.Get(),
                exceptionHandler: ex => stack.Push(ex));

            reaction.Get();

            Assert.AreEqual(1, stack.Count);
            Assert.AreEqual(exception, stack.Peek());
            Assert.Throws<Exception>(() => middle.Get());

            source.Value = 1;
            tick(0);

            Assert.AreEqual(2, middle.Value);
        });

        [Test]
        public void Invalidate() => TestZone.Run(tick =>
        {
            //
            var source = Atom.Value(0);

            string actualization = "";

            Atom.RunReaction(() =>
            {
                source.Get();
                actualization += "T";
            });

            tick(0);
            Assert.AreEqual("T", actualization);

            source.Invalidate();
            
            tick(0);
            Assert.AreEqual("TT", actualization);
        });

        [Test]
        public void WhenAtom() => TestZone.Run(tick =>
        {
            var source = Atom.Value(0);

            string watch = "";

            Atom.When(() => source.Value > 1)
                .Then(() => watch += "B");

            tick(0);
            Assert.AreEqual("", watch);

            source.Value = 1;
            tick(0);
            Assert.AreEqual("", watch);

            source.Value = 2;
            tick(0);
            Assert.AreEqual("B", watch);

            source.Value = 3;
            tick(0);
            Assert.AreEqual("B", watch);
        });

        class TestComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _comparison;

            public TestComparer(Func<T, T, bool> comparison) => _comparison = comparison;

            public bool Equals(T x, T y) => _comparison(x, y);
            public int GetHashCode(T obj) => obj.GetHashCode();
        }
    }
}