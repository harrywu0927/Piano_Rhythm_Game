
#include <iostream>
#include "MidiFile.h"
#include "Options.h"
#include <iomanip>
#include<fstream>
#include<unordered_map>
#include<string>
#include<io.h>
using namespace std;
//using namespace smf;
unordered_map<int, string> pitchmap{
        {0,"C"},{1,"C#"},{2,"D"},{3,"D#"},{4,"E"},{5,"F"},{6,"F#"},{7,"G"},{8,"G#"},{9,"A"},
        {10,"A#"}, {11,"B"}
};
void getAllFiles(string path, vector<string>& files, string fileType)
{
    // 文件句柄
    long hFile = 0;
    // 文件信息
    struct _finddata_t fileinfo;

    string p;

    if ((hFile = _findfirst(p.assign(path).append("\\*" + fileType).c_str(), &fileinfo)) != -1) {
        do {
            // 保存文件的全路径
            files.push_back(p.assign(path).append("\\").append(fileinfo.name));

        } while (_findnext(hFile, &fileinfo) == 0); //寻找下一个，成功返回0，否则-1

        _findclose(hFile);
    }
}


string getPitch(int value) {
    string pitch = pitchmap[value % 12];
    int level = value / 12 - 1;
    return pitch + to_string(level);
}
int main(int argc, const char * argv[]) {
    vector<string> files;
    getAllFiles("C:/Users/harry/Desktop/MIDIs", files, ".mid");
    for (int i = 0; i < files.size(); ++i)
    {
        
        cout << files[i] << endl;
        MidiFile midifile;
        midifile.read(files[i]);
        midifile.doTimeAnalysis();
        midifile.linkNotePairs();
        midifile.joinTracks();
        MidiEvent* mev;
        ofstream outfile;
        int flag = 0;
        while ((flag = files[i].find(".mid")) != -1) {
            files[i].erase(flag, 4);
        }
        outfile.open(files[i]+".txt", ios::out);
        if (!outfile.is_open())
            cout << "Open file failure" << endl;
        for (int event = 0; event < midifile[0].size(); event++) {
            mev = &midifile[0][event];
            if (mev->getDurationInSeconds() != 0) {
                if ((int)(*mev)[mev->size() - 1] != 0) {
                    outfile << mev->seconds;
                    outfile << " " << mev->getDurationInSeconds();
                    outfile << " ";
                    outfile << getPitch((int)(*mev)[mev->size() - 2]);
                    outfile << " " << (int)(*mev)[mev->size() - 1] << endl;
                }

            }
        }
        outfile.close();
    }
    /*MidiFile midifile;
    midifile.read("C:/Users/harry/Desktop/MIDIs/蒲公英的约定.mid");
    midifile.doTimeAnalysis();
    midifile.linkNotePairs();
    midifile.joinTracks();
    MidiEvent* mev;
    ofstream outfile;
    int flag = 0;
    outfile.open("C:/Users/harry/Desktop/MIDIs/蒲公英的约定.txt", ios::out);
    if (!outfile.is_open())
        cout << "Open file failure" << endl;
    for (int event = 0; event < midifile[0].size(); event++) {
        mev = &midifile[0][event];
        if (mev->getDurationInSeconds() != 0) {
            if ((int)(*mev)[mev->size() - 1] != 0) {
                outfile << mev->seconds;
                outfile << " " << mev->getDurationInSeconds();
                outfile << " ";
                outfile << getPitch((int)(*mev)[mev->size() - 2]);
                outfile << " " << (int)(*mev)[mev->size() - 1] << endl;
            }

        }
    }
    outfile.close();*/


    //打印一个midi event的列表
    //Options options;
    //MidiFile midifile;
    ////写文件的绝对路径
    //midifile.read("C:/Users/harry/Desktop/MIDIs/chno3701.mid");
    //midifile.doTimeAnalysis();
    //midifile.linkNotePairs();
    //int tracks = midifile.getTrackCount();
    //cout << "TPQ: " << midifile.getTicksPerQuarterNote() << endl;
    //if (tracks > 1) cout << "TRACKS: " << tracks << endl;
    //for (int track = 0; track < tracks; track++) {
    //    if (tracks > 1) cout << "\nTrack " << track << endl;
    //    cout << "Tick\tSeconds\tDur\tMessage" << endl;
    //    for (int event = 0; event < midifile[track].size(); event++) {
    //        cout << dec << midifile[track][event].tick;
    //        cout << '\t' << dec << midifile[track][event].seconds;
    //        cout << '\t';
    //        if (midifile[track][event].isNoteOn())
    //            cout << midifile[track][event].getDurationInSeconds();
    //        cout << '\t' << hex;
    //        for (int i = 0; i < midifile[track][event].size(); i++)
    //            cout << (int)midifile[track][event][i] << ' ';
    //        cout << endl;
    //    }
    //}
    return 0;
}

