// ======
//
// Copyright (c) Timothy Baxendale. All Rights Reserved.
//
// ======
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tbasic.Components
{
    internal class TrieNode<TKey, TValue>
    {
        private Lazy<Dictionary<TKey, TrieNode<TKey, TValue>>> edges;

        public TKey Name { get; set; }
        public IEqualityComparer<TKey> Comparer { get; }
        public TValue Value { get; set; }
        public bool IsEnd { get; set; }

        public Dictionary<TKey, TrieNode<TKey, TValue>> Children
        {
            get {
                return edges.Value;
            }
        }

        public TrieNode(TKey name, TValue value, IEqualityComparer<TKey> comparer)
        {
            Name = name;
            Value = value;
            Comparer = comparer;
            edges = new Lazy<Dictionary<TKey, TrieNode<TKey, TValue>>>(() => new Dictionary<TKey, TrieNode<TKey, TValue>>(Comparer));
        }
    }

    internal class Trie<TKey, TValue>
    {
        public Dictionary<TKey, TrieNode<TKey, TValue>> Edges { get; }

        public Trie(IEqualityComparer<TKey> comparer)
        {
            Edges = new Dictionary<TKey, TrieNode<TKey, TValue>>(comparer);
        }

        public void Add(IEnumerable<TKey> path, TValue value)
        {
            TrieNode<TKey, TValue> last = null;
            var dict = Edges;
            foreach(TKey val in path) {
                TrieNode<TKey, TValue> node = FindOrCreate(dict, val);
                dict = node.Children;
                last = node;
            }
            last.Value = value;
            last.IsEnd = true;
        }

        public bool Contains(IEnumerable<TKey> path)
        {
            TrieNode<TKey, TValue> last = null;
            var dict = Edges;
            foreach(TKey val in path) {
                TrieNode<TKey, TValue> node = Find(dict, val);
                if (node == null) {
                    return false;
                }
                else {
                    last = node;
                    dict = node.Children;
                }
            }
            return last.IsEnd;
        }

        private static TrieNode<TKey, TValue> Find(Dictionary<TKey, TrieNode<TKey, TValue>> edges, TKey name)
        {
            TrieNode<TKey, TValue> result;
            if (edges.TryGetValue(name, out result)) {
                return result;
            }
            else {
                return null;
            }
        }

        private static TrieNode<TKey, TValue> FindOrCreate(Dictionary<TKey, TrieNode<TKey, TValue>> edges, TKey name)
        {
            TrieNode<TKey, TValue> result;
            if (edges.TryGetValue(name, out result)) {
                return result;
            }
            else {
                return edges[name] = new TrieNode<TKey, TValue>(name, default(TValue), edges.Comparer);
            }
        }
    }
}
