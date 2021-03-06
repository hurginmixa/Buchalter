using System.Collections.Generic;

namespace Buchalter
{
    internal class WiresFile
    {
        private readonly List<IWire> _wires;
        private readonly string _fileName;

        public WiresFile(string fileName)
        {
            _fileName = fileName;

            _wires = Tools.LoadWires(_fileName);
        }

        public IReadOnlyList<IWire> Wires => _wires;

        public string FileName => _fileName;

        public void Insert(Wire wire)
        {
            _wires.Insert(0, wire);
        }
    }
}