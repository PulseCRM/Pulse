using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Globalization;
using System.Text;
using System.Diagnostics;

namespace Framework
{

    /// <summary>
    /// This class handles all the Point data retrieval and updates.
    /// </summary>
    public class FieldMap
    {
        public short FieldId;
        public string Value;
    }
    public class PNTLib
    {
        #region Properties
        const int cMaxNumFields = 31000;                                           // max number of fields supported
        //////////////////////////////////////////////////////////////////////////////
        public PNTLib()
        {
        }

        #endregion
        #region Initialize, lock the field map and array
        private void init(ref List<string> fieldArray, ref ArrayList fieldSeq)
        {
            int i;
            
            if (fieldArray == null)
                fieldArray = new List<string>();
            
            if (fieldSeq == null)
                fieldSeq = new ArrayList();
            
            fieldSeq.Clear();
            fieldArray.Clear();
            for (i = 0; i < cMaxNumFields; i++)
            {
                string temp = "";
                fieldArray.Add(temp);
            }
        }

        private bool BuildPointMap(ref List<string>fieldArray, ref ArrayList fieldSeq, byte[] buffer, int bufLen, ref string err)
        {
            // This method builds the arrays to store the length and field value retrieved from the Point file.
            // The sequence of Point data fields is also recorded so that when the Point file is updated 
            // the data field will be written back to the Point file in the same order
            int i, j, count = 0;
            short[] iFieldID = { 0, 0 };                                                // temporary variables to store field id, length 
            int[] tFieldLen = { 0, 0, 0, 0, 0, 0, 0 }, iFieldLen = { 0, 0,0,0,0,0, 0 };
            byte[] bFieldLen = { 0, 0, 0, 0, 0, 0, 0 };
            init(ref fieldArray, ref fieldSeq);
            byte[] data;
            if (bufLen <= 4)
                return true;

            System.Text.ASCIIEncoding en = new System.Text.ASCIIEncoding();
             try
            {
                i = 0; j = 1;
                do
                {
                    Buffer.BlockCopy(buffer, i, iFieldID, 0, 2);
                    if (iFieldID[0] == -1)
                        goto exit;                                      // end of Point file
                    i = i + 2;
                    Buffer.BlockCopy(buffer, i, bFieldLen, 0, 3);
                    i++;
                    if (bFieldLen[0] == 255)
                    {
                        if (bFieldLen[1] == 255 && bFieldLen[2] == 255)
                        {
                            i += 2;
                            Buffer.BlockCopy(buffer, i, iFieldLen, 0, 4);
                            i += 4;
                        }
                        else
                        {
                            Buffer.BlockCopy(buffer, i, iFieldLen, 0, 2);
                            i = i + 2;
                        }
                    }
                    else
                    {
                        iFieldLen[0] = bFieldLen[0];
                    }

                    data = new byte[iFieldLen[0]];
                    Buffer.BlockCopy(buffer, i, data, 0, iFieldLen[0]);
                    if (iFieldID[0] < cMaxNumFields)
                    {
                        string temp = en.GetString(data);
                        fieldArray[iFieldID[0]] = temp;
                        fieldSeq.Add(iFieldID[0]);
                    }
                    i = i + iFieldLen[0];
                    j++;
                    count++;

                } while ((i < bufLen) && (j < cMaxNumFields));
            }
            catch (Exception e)
            {
                err = "PNTLib BuildPointMap Exception: " + e.Message;
            }

        exit:
            //Console.WriteLine("BuildPointMap:: Copied a total of {0} Point fields. time={1}", count.ToString(), DateTime.Now.ToString());
            if (count > 0)
            {
                fieldSeq.Sort();
                return true;
            }
            else
                return false;
        }
        #endregion
        #region Update, Delete and Get Point Fields
        private bool DelPointField(ref List<string> fieldArray, ref ArrayList fieldSeq, int index)
        {
            if (index < 0)
                return true;

            if ((fieldArray == null) || (fieldSeq == null))
                throw new Exception("Point field maps fieldArray & fieldSeq are null.");
            try
            {
                if ((fieldArray.Count <= 0) || (fieldSeq.Count <= 0))
                    return true;

                short[] FID = { 0, 0 };

                FID[0] = (short)fieldSeq[index];

                fieldSeq.RemoveAt(index);
                if (FID[0] >= cMaxNumFields)
                    return true;

                fieldArray[FID[0]] = "";
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        public bool UpdPointField(ref List<string> fieldArray, ref ArrayList fieldSeq, short FID, string value, ref string err)
        {
            err = "";
            if (FID >= cMaxNumFields)                       // throw an exception if the FieldID is bigger than allowed
            {
                err = "The specified Field ID: " + FID.ToString() + " is too big; can support up to " + cMaxNumFields.ToString();
                return false;
            }
            try
            {
                if ((fieldArray == null) || (fieldSeq == null) || (fieldArray.Count <= 0))
                    init(ref fieldArray, ref fieldSeq);

                int index = fieldSeq.BinarySearch(FID);  // do a binary search on the list of Point Field IDs to see if it's there already
                if ((value.Trim().Length == 0) || (value == null))
                    return DelPointField(ref fieldArray, ref fieldSeq, index);

                if (index < 0)                                                      // if not found
                {
                    fieldSeq.Add(FID);
                    fieldSeq.Sort();                                  // sort the list after adding the Field ID
                }
                string temp = value.Trim();
                fieldArray[FID] = temp; // put the field value in the field array
            }
            catch (Exception ex)
            {
                err = "UpdatePointField, Exception: " + ex.Message;
                return false;
            }
            return true;
        }
        public string getPointField(List<string> fieldArray, ArrayList fieldSeq, short fieldID)
        {
            string value = "";
            if ((fieldArray == null) || (fieldSeq == null))
                throw new Exception ("getPointField: Point field maps are not set up.");
            if (fieldSeq.Count == 0)
                return value;

            if (fieldID >= cMaxNumFields)
            {
                throw new Exception("getPointField: specified Point Field Id, " + fieldID + " greater than maximum value.");
            }
            int index = (int)fieldID;
            System.Text.ASCIIEncoding en = new System.Text.ASCIIEncoding();
            if (fieldArray[index].Length <= 0)
                return null;
            value = fieldArray[index];
            return value;
        }
        #endregion
        #region Build the Point fields into a buffer
        public int RebuildPointData(List<string>fieldArray, ArrayList fieldSeq, ref List<byte> outputbuffer, ref string statusCode, ref string errorCode)
        {
            int lengthoffile;

            if ((fieldArray == null) || (fieldSeq == null))
                throw new Exception("RebuildPointData: Point field maps cannot be null.");
            System.Text.ASCIIEncoding en = new System.Text.ASCIIEncoding();
            lengthoffile = fieldSeq.Count;
            Trace.TraceInformation(string.Format("Start of RebuildPointData, Number of Fields: {0}.", fieldSeq.Count));
            Trace.Indent();
            if (lengthoffile <= 4)                                  // less than 4 fields in the Point Data Map
            {
                errorCode = "There is not enough data in the Point field maps; length is less than 4 bytes.";
                Trace.TraceError(string.Format("RebuildPointData, lengthoffile: {0}, Error:{1}.", lengthoffile, errorCode));
               
                return 0;
            }
            if (outputbuffer == null)
                outputbuffer = new List<byte>();
            //        Console.Write("RebuildPointData:: Starting to rebuild, time={0}", DateTime.Now.ToString());
            int i = 0, j = 0;
            short[] fieldID = { 0, 0 };
            short tempFieldID = 0;
            short[] fieldLen = { 0, 0, 0, 0, 0, 0 };
            int[] tFieldLen = { 0, 0 };
            int tempLen = 0;
            int writeCount = 0;
            byte[] temp;
            byte[] data;

            try
            {
                for (i = 0; i < lengthoffile; i++)
                {
                    j = 0;
                    tempLen = 3;
                    fieldID[0] = (short)fieldSeq[i];
                    tempFieldID = fieldID[0];
                    data = en.GetBytes(fieldArray[tempFieldID]);
                    if (data == null || data.Length <= 0)
                        continue;
                    //Trace.TraceInformation(string.Format("FieldID: {0}, Field Length: {1}", fieldSeq[i], data.Length));

                    if (data.Length >= Int16.MaxValue)
                    {
                        string strTemp = fieldArray[tempFieldID];
                        Trace.TraceError(string.Format("FieldID: {0}, Field Length: {1} has reached integer max value!!!", fieldSeq[i], data.Length));
                    }
                    tempLen += data.Length;
                    int fillerLength = 0;
                    fieldLen[0] = 0; fieldLen[1] = 0; fieldLen[1] = 0;
                    tFieldLen[0] = data.Length;
                    if (data.Length > 255)
                    {
                        if (data.Length <= Int16.MaxValue)
                        {
                            tempLen += 2;
                            fieldLen[0] = 255;
                            fieldLen[1] = (short)data.Length;
                            fillerLength = 1;
                        }
                        else
                        {
                            tempLen += 6;
                            fieldLen[0] = -1;
                            fieldLen[1] = -1;
                            fieldLen[2] = -1;
                            fillerLength = 3;
                            tFieldLen[0] = data.Length;
                        }
                    }
                    else
                    {
                        fieldLen[0] = (short)data.Length;
                        fieldLen[1] = (short)data.Length;
                    }
                    
                    // copy field ID - 2 bytes                   
                    temp = new byte[tempLen];
                    Buffer.BlockCopy(fieldID, 0, temp, j, 2);
                    j = j + 2;

                    // if length is >= 255, add 2 more bytes to length
                    // 1st byte = 255, 2nd+3rd bytes=length
                    if (fillerLength > 0)
                    {
                        Buffer.BlockCopy(fieldLen, 0, temp, j, fillerLength);
                        j += fillerLength;
                        if (data.Length > Int16.MaxValue)
                        {
                            Buffer.BlockCopy(tFieldLen, 0, temp, j, 4);
                            j += 4;
                        }
                        else if (data.Length > 255)
                        {
                            Buffer.BlockCopy(tFieldLen, 0, temp, j, 2);
                            j += 2;
                        }
                    }
                    else
                    {
                        Buffer.BlockCopy(fieldLen, 0, temp, j, 1);
                        j++;
                    }

                    Buffer.BlockCopy(data, 0, temp, j, data.Length);

                    outputbuffer.AddRange(temp);
                    //Console.WriteLine("RebuildPointMap::Copied fieldID:{0}, fieldLen:{1}.", fieldID[0].ToString(), data.Length.ToString());
                    writeCount = writeCount + j + data.Length;
                    Trace.TraceInformation(string.Format("RebuildPointData: after adding FieldID {0}, FieldLength: {1}, Total WriteCount={2}.", fieldID[0], data.Length, writeCount));
                }
            }
            catch (Exception e)
            {
                errorCode = "RebuildPointMap, Exception: " + e.ToString();
                Trace.TraceError(errorCode);
                writeCount = 0;
            }
            byte[] trailer = new byte[2];
            trailer[0] = 255; trailer[1] = 255;
            outputbuffer.AddRange(trailer);
            writeCount += 2;
            Trace.TraceInformation(string.Format("RebuildPointData, end: total Output Buffer Length={0}.", outputbuffer.Count));
            Trace.Flush();
            //    Console.WriteLine("RebuildPointMap:: Copied a total of {0} bytes, time={1}", writeCount.ToString(), DateTime.Now.ToString());
            return writeCount;
        }
        #endregion
        #region read Point data
        public bool ReadPointData(ref List<string>fieldArray, ref ArrayList fieldSeq, byte[] InputBuffer, ref string err)
        {
            err = "";
            if (InputBuffer.Length <= 4)
            {
                err = "ReadPointData, the Input Buffer is too small. It must be greater than 4 bytes.";
                return false;
            }
            try
            {
                return BuildPointMap(ref fieldArray, ref fieldSeq, InputBuffer, InputBuffer.Length, ref err);
            }
            catch (Exception ex)
            {
                err = "ReadPointData, Exception:" + ex.Message + ex.StackTrace;
                return false;
            }
        }

        public bool ReadPointData(ref List<string>fieldArray, ref ArrayList fieldSeq, string fpath, ref string err)
        {
            bool status = true;
            int lengthoffile;                          // length of the Point file
            err = "";
            //Prepare to open file

            // if the file doesn't exist, terminate
            if (File.Exists(fpath) == false)
            {
                err = "The specified Point file: " + fpath + " does not exist.";
                return false;
            }
            using (FileStream fs = new FileStream(fpath, FileMode.Open, FileAccess.Read))
            {
                byte[] buffer;
                int read = 0;
                lengthoffile = (int)fs.Length;
                if (lengthoffile <= 4)
                {
                    err = "The specified Point file: " + fpath + " does not have enough data; length is less than 4 bytes";
                }
                buffer = new byte[lengthoffile + 1];
                try
                {
                    // read until Read method returns 0 (end of the stream has been reached)
                    read = fs.Read(buffer, 0, lengthoffile);
                    if (read == 0)
                    {
                        status = false;
                    }

                    status = BuildPointMap(ref fieldArray, ref fieldSeq, buffer, read, ref err);
                }
                catch (Exception e)
                {
                    //              Console.WriteLine("ReadPointData::Exception: {0}", e.Message);
                    err = "Exception: " + e.Message;
                    status = false;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }
            }
            return status;
        }

        #endregion
        #region Write Point data
        public bool WritePointData(List<FieldMap> UpdatedPointFields, string filePath, ref string err)
        {
            err = "";
            FileStream fs = null;
            BinaryWriter bw = null;
            try
            {
                if ((UpdatedPointFields == null) || (filePath == null) || (filePath == string.Empty))
                {
                    err = "The parameter(s) fieldArray, fieldSeq, UpdatedPointFields, or filePath may be null.";
                    return false;
                }
                if (UpdatedPointFields.Count <= 0)
                {
                    err = "The UpdatedPointFields list is empty.";
                    return false;
                }
                string folder = "";
                if (FileHelper.IsFilenameValid(filePath, ref folder, ref err) == false)
                    return false;
                List<string> FieldArray = new List<string>();
                ArrayList FieldSeq = new ArrayList();
                if (ReadPointData(ref FieldArray, ref FieldSeq, filePath, ref err) == false)
                    return false;
                using (fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                {
                    foreach (FieldMap fm in UpdatedPointFields)
                    {
                        if (fm.FieldId <= 0)
                            continue;
                        if (UpdPointField(ref FieldArray, ref FieldSeq, fm.FieldId, fm.Value, ref err) == false)
                            continue;
                    }
                    string errCode = "";
                    List<byte> outBuf = new List<byte>();
                    int len;
                    len = RebuildPointData(FieldArray, FieldSeq, ref outBuf, ref  errCode, ref err);
                    if (len <= 4)
                    {
                        err = "Output is less than 4 bytes.";
                        return false;
                    }
                    bw = new BinaryWriter(fs);
                    bw.Write(outBuf.ToArray());
                    bw.Flush();
                    return true;
                }
            }
            catch (Exception ex)
            {
                err = "WritePointData, Exception: " + ex.Message;
                return false;
            }
            finally
            {
                if (bw != null)
                {
                    bw.Close();
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }

        }

        public bool WritePointData(List<string> fieldArray, ArrayList fieldSeq, FileStream fs, List<FieldMap> UpdatedPointFields, ref string err)
        {
            err = "";
            if (fs == null)
            {
                err = "WritePointData, FileStream is null.";
                return false;
            }
            if ((UpdatedPointFields == null) || (UpdatedPointFields.Count <= 0))
            {
                err = "WritePointData, UpdatePointField List is empty. ";
                return false;
            }
            byte[] buffer;
            int read = 0;
            int lengthoffile = (int)fs.Length;
            if (lengthoffile <= 4)
            {
                err = "WritePointData, the file does not have enough data; length is less than 4 bytes";
                return false;
            }
            BinaryWriter bw = null;
            buffer = new byte[lengthoffile + 1];
            try
            {
                // read until Read method returns 0 (end of the stream has been reached)
                read = fs.Read(buffer, 0, lengthoffile);

                if (BuildPointMap(ref fieldArray, ref fieldSeq, buffer, read, ref err) == false)
                {
                    return false;
                }
                foreach (FieldMap fm in UpdatedPointFields)
                {
                    if (fm.FieldId <= 0)
                     continue;
                    if (UpdPointField(ref fieldArray, ref fieldSeq, fm.FieldId, fm.Value, ref err) == false)
                     continue;
                }
                 string errCode = "";
                 List<byte> outBuf = new List<byte>();
                 int len;
                 len = RebuildPointData(fieldArray, fieldSeq, ref outBuf, ref  errCode, ref err);
                 if (len <= 4)
                 {
                     err = "Output is less than 4 bytes.";
                     return false;
                 }
                 bw = new BinaryWriter(fs);
                 bw.Write(outBuf.ToArray());
                 return true;
            }
            catch (Exception e)
            {
                 err = "Exception: " + e.Message;
                return false;
            }
            finally
            {
                if (bw != null)
                {
                    bw.Close();
                    bw = null;
                }
            }
        }

        public bool WritePointData(List<string>fieldArray, ArrayList fieldSeq, out byte[] Outputbuffer, ref string err)
        {
            err = "";
            string errCode = "";
            Outputbuffer = null;
            List<byte> outBuf = new List<byte>();
            int len;
            len = RebuildPointData(fieldArray, fieldSeq, ref outBuf, ref  errCode, ref err);
            if (len <= 0)
                return false;
            if ((outBuf == null) || (outBuf.Count <= 0))
                return false;
            Outputbuffer = outBuf.ToArray();
            return true;
        }

        public bool WritePointData(List<string>fieldArray, ArrayList fieldSeq, string fpath, bool createFile, byte[] buffer, ref string statusCode, ref string errorCode)
        {
            // This method writes the Point data fields to the specified Point file. 
            // If the createFile flag is true, it will create a new file if it does not exist; otherwise, return false.
            // Since, we'll not be creating a Point file from scratch, it will always use the Point data retrieved from the file and then apply updates if needed. 

            FileStream fs = null;
            BinaryWriter bw = null;
            FileMode fm;

            if (fpath == "")    // an empty file path??
            {
                errorCode = "The specified file path is null.";
                return false;
            }

            if (createFile == true)
                fm = FileMode.OpenOrCreate;
            else
                fm = FileMode.Open;

            if (File.Exists(fpath) == false)
            {
                if (createFile == false)
                {
                    errorCode = "The specified file does not exist and the CreateFile option is not on.";
                    return false;
                }
            }
            using (fs = new FileStream(fpath, fm, FileAccess.ReadWrite))
            {
                try
                {
                    if (buffer.Length <= 0)
                    {
                        return false;
                    }
                    bw = new BinaryWriter(fs);
                    bw.Write(buffer);
                    return true;
                }
                catch (Exception e)
                {
                    errorCode = "WritePointData, got an exception writing Point file " + fpath + ", Exception:" + e.Message;
                    return false;
                }
                finally
                {
                    if (bw != null)
                    {
                        bw.Close();
                        bw = null;
                    }
                    if (fs != null)
                    {
                        fs.Close();
                        fs.Dispose();
                    }
                }
            }
        }
        #endregion
    }
}