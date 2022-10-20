using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // ���݂̏��
    PlayState state;

    // �Q�[�����̃p�l��
    [SerializeField]
    GameObject playingPanel;
    // �v���C���[������(�Q�[�����n�܂�O�̑ҋ@���)�p�̃p�l��
    [SerializeField]
    GameObject inputSelectingPanel;
    // ���[�h���̕\���p�p�l��
    [SerializeField]
    GameObject RoadingPanel;
    // ���U���g�\���p�̃p�l��
    [SerializeField]
    GameObject resultingPanel;
    // �����L���O�\���p�̃p�l��
    [SerializeField]
    GameObject rankingPanel;

    // �Q�[����ʂŎg�p����e�L�X�g�Ɖ摜
    [SerializeField]
    Text[] PlayingText;
    [SerializeField]
    Image[] PlayingImage;

    // �ҋ@�I����ʂŎg�p����e�L�X�g�Ɖ摜
    [SerializeField]
    Text[] InputSelectingText;
    [SerializeField]
    Image[] InputSelectingImage;

    // ���[�h��ʂŎg�p����e�L�X�g�Ɖ摜
    [SerializeField]
    Text[] RoadingText;
    [SerializeField]
    Image[] RoadingImage;

    // ���U���g��ʂŎg�p����e�L�X�g�Ɖ摜
    [SerializeField]
    Text[] ResultingName;
    [SerializeField]
    Text[] ResultingScore;
    [SerializeField]
    Image[] ResultingImage;

    // �����L���O��ʂŎg�p����e�L�X�g�Ɖ摜
    [SerializeField]
    Text[] RankingName;
    [SerializeField]
    Text[] RankingScore;
    [SerializeField]
    Text[] RankingAroundRank;
    [SerializeField]
    Text[] RankingAroundName;
    [SerializeField]
    Text[] RankingAroundScore;
    [SerializeField]
    Image[] RankingImage;

    bool[] stateInit;
    bool finishFrag = false;

    void Start()
    {
        state = PlayState.Resulting;
        InitUI();
    }

    async void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
		{
            var result = await ServerRequestController.GetRanking();
            string s = "";
            foreach(var user in result.Users)
            {
                s += user.name + "\n";
            }
            Debug.Log(s);
        }
        switch(state)
        {
            // �Q�[����ʂł̖��t���[������
            case PlayState.Playing:
                // �ŏ��̈��ڂ̂ݎ��s(���\�b�h����true��)
                if(!stateInit[0])
                    InitPlayerUI();
                UpdatePlayingUI();
                break;
            // ���f���̏���
            case PlayState.Paused:
                
                break;
            // ���͑I����ʂ̏���
            case PlayState.InputSelecting:
                // �ŏ��̈��ڂ̂ݎ��s(���\�b�h����true��)
                if(!stateInit[1])
                    InitInputSelectingUI();
                UpdateInputSelectingUI();
                break;
            // ���[�h��ʂ̏���
            case PlayState.Roading:
                // �ŏ��̈��ڂ̂ݎ��s(���\�b�h����true��)
                if(!stateInit[2])
                    InitRoadingUI();
                UpdateRoadingUI();
                break;
            // ���U���g��ʂ̏���
            case PlayState.Resulting:
                //  �ŏ��̈��ڂ̂ݎ��s(���\�b�h����true��)
                if(!stateInit[3])
                    InitResultingUI();
                UpdateResultingUI();
                break;
            // �����L���O��ʂ̏���
            case PlayState.Ranking:
                // �ŏ��̈��ڂ̂ݎ��s(���\�b�h����true��)
                if(!stateInit[4])
                    InitRankingUI();
                UpdateRankingUI();
                
                break;
            default:
                break;
        }
    }
    // UI�̏����ݒ�
    void InitUI()
	{
        stateInit = new bool[5];
	}

    // �Q�[�����̕`��X�V
    void UpdatePlayingUI()
	{

	}
    // �Q�[����ʂ̏����ݒ�
    void InitPlayerUI()
    {
        // �����ݒ肵���̂�true
        stateInit[0] = true;
	}

    // �ҋ@��ʂ̕`��X�V
    void UpdateInputSelectingUI()
	{

	}
    // �ҋ@��ʂ̏����ݒ�
    void InitInputSelectingUI()
    {
        // �����ݒ肵���̂�true
        stateInit[1] = true;
	}

    // ���[�h��ʂ̕`��X�V
    void UpdateRoadingUI()
	{

	}
    // ���[�h��ʂ̏����ݒ�
    void InitRoadingUI()
    {
        // �����ݒ肵���̂�true
        stateInit[2] = true;

	}

    // ���U���g�̕`��X�V
    void UpdateResultingUI()
    {

    }
    // ���U���g��ʂ̏����ݒ�
    async void InitResultingUI()
    {
        // �����ݒ肵���̂�true
        stateInit[3] = true;

        // ���U���g�p�l����\������ȊO���\��
        SetPanelActives(PlayState.Ranking);

        // 
        foreach(var dic in PlayerData.DictionaryID)
        {
            var result = await ServerRequestController.GetScore(dic.Value);
        }
    }

    // �����L���O��ʂ̕`��X�V
    void UpdateRankingUI()
	{
        if(finishFrag)
		{
            state = PlayState.Roading;
		}
	}
    // �����L���O��ʂ̏����ݒ�
    async void InitRankingUI()
	{
        // �����ݒ肵���̂�true
        stateInit[4] = true;

        // �����L���O�p�l����\������ȊO���\��
        SetPanelActives(PlayState.Ranking);
        
        // �����L���O��ʂ���擾
        var result = await ServerRequestController.GetRanking();

        // �����L���O����ʂ���8���ŕ\��
        for(int i = 0;i < 8; i++)
		{
            RankingName[i].text = result.Users[i].name;
            RankingScore[i].text = result.Users[i].rate.ToString();
		}
        // ���[�U���ӂ̃����L���O��\��
        var result2 = await ServerRequestController.GetUserRanking(PlayerData.UserId);

        // ��������̏��ʂ̐l�̐�
        int higherCount = result2.higher_around_rank_users.Length;
        // ������艺�̏��ʂ̐l�̐�
        int lowerCount = result2.lower_around_rank_users.Length;

        // ���ӏ��ʂ̕\���͏㉺2�l���Ȃ̂Ő���2�ȏ�Ȃ�2��2�����Ȃ琔������
        // �eTEXT�z���[0-1]��ʁA[2]�����A[3-4]���ʂ̍\��
        for(int i = 0; i < (higherCount < 2 ? higherCount : 2); i++)
		{
            RankingAroundRank[1 - i].text = result2.higher_around_rank_users[higherCount - i - 1].rank.ToString();
            RankingAroundName[1 - i].text = result2.higher_around_rank_users[higherCount - i - 1].name;
            RankingAroundScore[1 - i].text = result2.higher_around_rank_users[higherCount - i - 1].rate.ToString();
        }
        // ����
        for(int j = 0; j < (lowerCount < 2 ? lowerCount : 2); j++)
		{
            // �z��̈�����[3-4]�����邽�߂�3�𑫂�
            RankingAroundRank[3 + j].text = result2.lower_around_rank_users[j].rank.ToString();
            RankingAroundName[3 + j].text = result2.lower_around_rank_users[j].name;
            RankingAroundScore[3 + j].text = result2.lower_around_rank_users[j].rate.ToString();
        }
        // �����̏��ʂƖ��O�A�X�R�A��^�񒆂ɕ\��[2]
        RankingAroundRank[2].text = result2.self.rank.ToString();
        RankingAroundName[2].text = result2.self.name;
        RankingAroundScore[2].text = result2.self.rate.ToString();
    }

    void SetPanelActives(PlayState playState)
	{
        playingPanel.SetActive(state == playState);
        inputSelectingPanel.SetActive(state == playState);
        RoadingPanel.SetActive(state == playState);
        rankingPanel.SetActive(state == playState);
        resultingPanel.SetActive(state == playState);
    }

    // ���݂̏�Ԃ�\���񋓌^
    // 0 �Q�[����
    // 1 ���f��
    // 2 ���[�h��
    // 3 �ҋ@���
    // 4 ���U���g
    // 5 �����L���O
    public enum PlayState
	{
        Playing = 0,
        Paused = 1,
        Roading = 2,
        InputSelecting = 3,
        Resulting = 4,
        Ranking = 5
	}
}
